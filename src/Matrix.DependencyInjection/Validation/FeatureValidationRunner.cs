using System.Reflection;
using System.Runtime.InteropServices;

namespace Matrix.DependencyInjection.Validation;

public sealed class FeatureValidationRunner(
    MatrixModule module,
    IMatrixReportStore reportStore) : IMatrixRunner
{
    public string DefaultOutputFile =>
        Path.Combine("reports", module.ReportDirectory, "features.json");

    public int Run(IReadOnlyList<MatrixLibrary> libraries, RunnerOptions options)
    {
        var featureTypes = typeof(FeatureValidationRunner).Assembly
            .GetTypes()
            .Select(type => (
                Type: type,
                Feature: type
                    .GetCustomAttributes(typeof(FeatureBenchmarkAttribute), false)
                    .Cast<FeatureBenchmarkAttribute>()
                    .SingleOrDefault()))
            .Where(item => item.Feature is not null)
            .OrderBy(item => item.Feature!.Order)
            .ToArray();
        var capturedResults = new List<CapturedFeatureResult>();
        var successful = true;

        foreach (var library in libraries.OrderBy(i => i.Id, StringComparer.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Validating {library.Name} {library.Version}");
            foreach (var item in featureTypes)
            {
                var feature = item.Feature!;
                var method = item.Type
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .SingleOrDefault(candidate =>
                        candidate.GetCustomAttribute<LibraryBenchmarkAttribute>()?.LibraryId == library.Id);
                if (method is null)
                {
                    var unavailable = item.Type
                        .GetCustomAttributes<FeatureUnavailableAttribute>()
                        .SingleOrDefault(attribute => attribute.LibraryId == library.Id);
                    var status = unavailable?.Status ?? FeatureStatus.Failed;
                    var reason = unavailable?.Reason ?? "No direct benchmark method or availability metadata exists.";
                    if (status == FeatureStatus.Failed)
                    {
                        successful = false;
                    }

                    capturedResults.Add(CreateResult(library.Id, feature, status, reason, 0));
                    Console.WriteLine($"  {feature.Order:00} {feature.Name}: {status}");
                    continue;
                }

                if (method.ReturnType == typeof(object))
                {
                    successful = false;
                    const string reason = "A benchmark method must not return System.Object.";
                    capturedResults.Add(CreateResult(
                        library.Id,
                        feature,
                        FeatureStatus.Failed,
                        reason,
                        0));
                    Console.Error.WriteLine($"  {feature.Order:00} {feature.Name}: Failed - {reason}");
                    continue;
                }

                var stopwatch = Stopwatch.StartNew();
                object? instance = null;
                try
                {
                    instance = Activator.CreateInstance(item.Type)
                               ?? throw new InvalidOperationException($"Cannot create {item.Type.FullName}.");
                    InvokeIfExists(item.Type, instance, $"Setup{method.Name}");
                    for (var iteration = 0; iteration < 3; iteration++)
                    {
                        method.Invoke(instance, null);
                    }

                    stopwatch.Stop();
                    capturedResults.Add(CreateResult(
                        library.Id,
                        feature,
                        FeatureStatus.Supported,
                        null,
                        stopwatch.Elapsed.TotalMilliseconds));
                    Console.WriteLine($"  {feature.Order:00} {feature.Name}: Supported");
                }
                catch (Exception exception)
                {
                    stopwatch.Stop();
                    successful = false;
                    var actual = exception is TargetInvocationException { InnerException: not null }
                        ? exception.InnerException
                        : exception;
                    var reason = $"{actual?.GetType().Name}: {actual?.Message}";
                    capturedResults.Add(CreateResult(
                        library.Id,
                        feature,
                        FeatureStatus.Failed,
                        reason,
                        stopwatch.Elapsed.TotalMilliseconds));
                    Console.Error.WriteLine($"  {feature.Order:00} {feature.Name}: Failed - {reason}");
                }
                finally
                {
                    if (instance is not null)
                    {
                        try
                        {
                            InvokeIfExists(item.Type, instance, $"Cleanup{method.Name}");
                        }
                        catch (Exception cleanupException)
                        {
                            successful = false;
                            Console.Error.WriteLine(
                                $"  {feature.Order:00} {feature.Name}: cleanup failed - {cleanupException.Message}");
                        }
                    }
                }
            }

        }

        var features = capturedResults
            .GroupBy(result => (result.Order, result.Id, result.Name))
            .Select(group => new FeatureReportEntry(
                group.Key.Order,
                group.Key.Id,
                group.Key.Name,
                [
                    .. group
                        .Select(result => result.Result)
                        .OrderBy(result => result.LibraryId, StringComparer.OrdinalIgnoreCase)
                ]))
            .OrderBy(feature => feature.Order)
            .ToArray();
        var report = new FeatureReport(
            1,
            module.Id,
            DateTimeOffset.UtcNow,
            RuntimeInformation.FrameworkDescription,
            RuntimeInformation.OSDescription,
            [.. libraries.OrderBy(library => library.Id, StringComparer.OrdinalIgnoreCase)],
            features);
        var isPartial = libraries.Count != module.Libraries.Count;
        if (isPartial)
        {
            var existing = reportStore.Read<FeatureReport>(options.OutputFile);
            if (existing is not null)
            {
                report = Merge(existing, report, libraries);
            }
        }

        reportStore.Write(options.OutputFile, report);
        Console.WriteLine($"Feature report: {Path.GetFullPath(options.OutputFile)}");
        return successful ? 0 : 1;
    }

    private static void InvokeIfExists(Type type, object instance, string methodName) =>
        type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public)?.Invoke(instance, null);

    private static CapturedFeatureResult CreateResult(
        string libraryId,
        FeatureBenchmarkAttribute feature,
        FeatureStatus status,
        string? reason,
        double durationMilliseconds) =>
        new(
            feature.Order,
            feature.Id.ToString(),
            feature.Name,
            new FeatureResult(
                libraryId,
                status.ToString(),
                reason,
                durationMilliseconds));

    private FeatureReport Merge(
        FeatureReport existing,
        FeatureReport current,
        IReadOnlyList<MatrixLibrary> selectedLibraries)
    {
        if (existing.ModuleId is not null
            && !existing.ModuleId.Equals(module.Id, StringComparison.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine(
                $"WARNING: Existing feature report belongs to module '{existing.ModuleId}', "
                + $"not '{module.Id}'. It will be replaced by the partial result.");
            return current;
        }

        var selectedIds = selectedLibraries
            .Select(library => library.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var mergedLibraries = existing.Libraries
            .Where(library => !selectedIds.Contains(library.Id))
            .Concat(current.Libraries)
            .OrderBy(library => library.Id, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var currentFeatures = current.Features
            .ToDictionary(feature => feature.Id, StringComparer.Ordinal);
        var mergedFeatures = existing.Features
            .Select(feature =>
            {
                if (!currentFeatures.TryGetValue(feature.Id, out var currentFeature))
                {
                    return feature;
                }

                return currentFeature with
                {
                    Results =
                    [
                        .. feature.Results
                            .Where(result => !selectedIds.Contains(result.LibraryId))
                            .Concat(currentFeature.Results)
                            .OrderBy(result => result.LibraryId, StringComparer.OrdinalIgnoreCase)
                    ]
                };
            })
            .Concat(current.Features.Where(feature =>
                existing.Features.All(existingFeature => existingFeature.Id != feature.Id)))
            .OrderBy(feature => feature.Order)
            .ToArray();
        return current with
        {
            Libraries = mergedLibraries,
            Features = mergedFeatures
        };
    }

    private sealed record CapturedFeatureResult(
        int Order,
        string Id,
        string Name,
        FeatureResult Result);
}
