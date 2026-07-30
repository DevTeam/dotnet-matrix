using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System.Reflection;

// ReSharper disable UseCollectionExpression
namespace Matrix;

public sealed class MatrixBenchmarkRunner(
    MatrixModule module,
    MatrixModuleAssembly moduleAssembly,
    IMatrixReportStore reportStore,
    IBenchmarkEnvironmentProvider environmentProvider) : IMatrixRunner
{
    public string DefaultOutputFile =>
        Path.Combine("reports", module.ReportDirectory, "benchmarks.json");

    public int Run(IReadOnlyList<MatrixLibrary> libraries, RunnerOptions options)
    {
        var runLibraries = IncludeBaselines(libraries);
        var outputPath = Path.GetFullPath(options.OutputFile);
        var artifactsDirectory = Path.Combine(
            Path.GetTempPath(),
            "dotnet-matrix",
            module.ReportDirectory,
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(artifactsDirectory);

        var jobId = options.Smoke ? "Smoke" : "Quick";
        var environment = environmentProvider.Capture(
            "BenchmarkDotNet",
            typeof(BenchmarkSwitcher).Assembly,
            jobId);
        var isPartial = runLibraries.Count != module.Libraries.Count;
        BenchmarkReport? existing = null;
        if (isPartial)
        {
            existing = reportStore.Read<BenchmarkReport>(outputPath);
            if (existing is not null)
            {
                if (existing.ModuleId is not null
                    && !existing.ModuleId.Equals(module.Id, StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine(
                        $"WARNING: Existing benchmark report belongs to module '{existing.ModuleId}', "
                        + $"not '{module.Id}'. It will be replaced by the partial result.");
                    existing = null;
                }
                else
                {
                    reportStore.WarnEnvironmentMismatch(
                        existing.Environments ?? [],
                        environment);
                }
            }
        }

        var job = Job.Default
            .WithId(jobId)
            .WithArguments([new MsBuildArgument("/p:MatrixMode=Benchmark")]);
        job = options.Smoke
            ? job
                .WithWarmupCount(1)
                .WithIterationCount(1)
                .WithInvocationCount(1)
                .WithUnrollFactor(1)
            : job
                .WithWarmupCount(2)
                .WithIterationCount(5);

        var ids = runLibraries
            .Select(library => library.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var config = ManualConfig.Create(DefaultConfig.Instance)
            .AddFilter(new MatrixBenchmarkFilter(ids))
            .AddJob(job)
            .AddExporter(JsonExporter.Full)
            .WithArtifactsPath(artifactsDirectory);
        try
        {
            var summaries = BenchmarkSwitcher
                .FromAssembly(moduleAssembly.Value)
                .Run(["--filter", "*"], config);

            var measuredResults = summaries
                .SelectMany(summary => summary.Reports)
                .Select(report =>
                {
                    var method = report.BenchmarkCase.Descriptor.WorkloadMethod;
                    var library = method.GetCustomAttribute<LibraryBenchmarkAttribute>()!;
                    var feature = method.DeclaringType!.GetCustomAttribute<MatrixFeatureAttribute>()!;
                    var payloadSize = method.GetCustomAttribute<PayloadSizeAttribute>();
                    var allocatedBytes = report.Metrics.TryGetValue("Allocated Memory", out var allocated)
                        ? allocated.Value
                        : (double?)null;
                    return new CapturedBenchmarkResult(
                        feature.Order,
                        feature.Id,
                        feature.Name,
                        new BenchmarkResult(
                            library.LibraryId,
                            report.Success,
                            report.ResultStatistics?.Mean,
                            report.ResultStatistics?.StandardError,
                            allocatedBytes,
                            environment.Id,
                            payloadSize?.Bytes));
                })
                .ToArray();
            var reportedResults = CaptureReportedResults(runLibraries, environment.Id);
            var capturedResults = measuredResults
                .Concat(reportedResults)
                .ToArray();
            var features = capturedResults
                .GroupBy(result => (result.Order, result.Id, result.Name))
                .Select(group => new BenchmarkReportEntry(
                    group.Key.Order,
                    group.Key.Id,
                    group.Key.Name,
                    group
                        .Select(result => result.Result)
                        .OrderBy(result => result.LibraryId, StringComparer.OrdinalIgnoreCase)
                        .ToArray()))
                .OrderBy(feature => feature.Order)
                .ToArray();
            var benchmarkLibraries = runLibraries
                .Select(library => new BenchmarkLibrary(
                    library.Id,
                    library.Name,
                    library.Package,
                    library.Version,
                    library.Baseline))
                .OrderBy(library => library.Id, StringComparer.OrdinalIgnoreCase)
                .ToArray();
            var successful = capturedResults.Length > 0
                             && capturedResults.All(result => result.Result.Successful);
            var report = new BenchmarkReport(
                1,
                module.Id,
                DateTimeOffset.UtcNow,
                [environment],
                benchmarkLibraries,
                features);
            if (existing is not null)
            {
                report = Merge(existing, report, runLibraries);
            }

            reportStore.Write(outputPath, report);
            Console.WriteLine($"Benchmark report: {outputPath}");
            return successful ? 0 : 1;
        }
        finally
        {
            if (Directory.Exists(artifactsDirectory))
            {
                Directory.Delete(artifactsDirectory, true);
            }
        }
    }

    private IReadOnlyList<MatrixLibrary> IncludeBaselines(
        IReadOnlyList<MatrixLibrary> selectedLibraries)
    {
        if (selectedLibraries.All(library => library.Baseline))
        {
            return selectedLibraries;
        }

        return selectedLibraries
            .Concat(module.Libraries.Where(library => library.Baseline))
            .DistinctBy(library => library.Id, StringComparer.OrdinalIgnoreCase)
            .OrderBy(library => library.Id, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static BenchmarkReport Merge(
        BenchmarkReport existing,
        BenchmarkReport current,
        IReadOnlyList<MatrixLibrary> selectedLibraries)
    {
        var selectedIds = selectedLibraries
            .Select(library => library.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var currentFeatures = current.Features
            .ToDictionary(feature => feature.Id, StringComparer.Ordinal);
        var features = existing.Features
            .Select(feature =>
            {
                if (!currentFeatures.TryGetValue(feature.Id, out var currentFeature))
                {
                    return feature;
                }

                return currentFeature with
                {
                    Results = feature.Results
                        .Where(result => !selectedIds.Contains(result.LibraryId))
                        .Concat(currentFeature.Results)
                        .OrderBy(result => result.LibraryId, StringComparer.OrdinalIgnoreCase)
                        .ToArray()
                };
            })
            .Concat(current.Features.Where(feature =>
                existing.Features.All(existingFeature => existingFeature.Id != feature.Id)))
            .OrderBy(feature => feature.Order)
            .ToArray();
        var environmentIds = features
            .SelectMany(feature => feature.Results)
            .Select(result => result.EnvironmentId)
            .Where(id => id is not null)
            .ToHashSet(StringComparer.Ordinal);
        var environments = (existing.Environments ?? [])
            .Concat(current.Environments ?? [])
            .Where(environment => environmentIds.Contains(environment.Id))
            .DistinctBy(environment => environment.Id, StringComparer.Ordinal)
            .OrderBy(environment => environment.Id, StringComparer.Ordinal)
            .ToArray();
        var libraries = existing.Libraries
            .Where(library => !selectedIds.Contains(library.Id))
            .Concat(current.Libraries)
            .DistinctBy(library => library.Id, StringComparer.OrdinalIgnoreCase)
            .OrderBy(library => library.Id, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        return current with
        {
            Environments = environments,
            Libraries = libraries,
            Features = features
        };
    }

    private CapturedBenchmarkResult[] CaptureReportedResults(
        IReadOnlyList<MatrixLibrary> libraries,
        string environmentId)
    {
        var libraryIds = libraries
            .Select(library => library.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        return moduleAssembly.Value
            .GetTypes()
            .SelectMany(type =>
            {
                var feature = type.GetCustomAttribute<MatrixFeatureAttribute>();
                if (feature is null)
                {
                    return [];
                }

                return type
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Select(method => (
                        Method: method,
                        Feature: feature,
                        Library: method.GetCustomAttribute<LibraryBenchmarkAttribute>(),
                        Reported: method.GetCustomAttribute<ReportedBenchmarkAttribute>()))
                    .Where(item =>
                        item.Library is not null
                        && item.Reported is not null
                        && libraryIds.Contains(item.Library.LibraryId));
            })
            .Select(item => new CapturedBenchmarkResult(
                item.Feature.Order,
                item.Feature.Id,
                item.Feature.Name,
                new BenchmarkResult(
                    item.Library!.LibraryId,
                    true,
                    item.Reported!.MeanNanoseconds,
                    0,
                    item.Reported.AllocatedBytesPerOperation,
                    environmentId,
                    item.Method.GetCustomAttribute<PayloadSizeAttribute>()?.Bytes)))
            .ToArray();
    }
}
