using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;
using System.Security.Cryptography;
using System.Reflection;
using System.Text.Json;

// ReSharper disable UseCollectionExpression
namespace Matrix;

public sealed class MatrixBenchmarkRunner(
    MatrixModule module,
    MatrixModuleAssembly moduleAssembly,
    IMatrixReportStore reportStore,
    IBenchmarkEnvironmentProvider environmentProvider,
    IJsonSerializer jsonSerializer) : IMatrixRunner
{
    private static readonly JsonSerializerOptions EvidenceJsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

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
        var evidenceBaseId = CreateEvidenceId(module.Id);
        var measuredEvidenceId = $"{evidenceBaseId}-measured";
        var reportedEvidenceId = $"{evidenceBaseId}-reported";
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
        // Preserve roughly the old 2 + 5 iterations at 500 ms time budget,
        // but collect more samples and let BenchmarkDotNet adapt to noisy cases.
        job = options.Smoke
            ? job
                .WithWarmupCount(1)
                .WithIterationCount(1)
                .WithInvocationCount(1)
                .WithUnrollFactor(1)
            : job
                .WithMinWarmupCount(3)
                .WithMaxWarmupCount(5)
                .WithIterationTime(TimeInterval.Millisecond * 250)
                .WithMinIterationCount(8)
                .WithMaxIterationCount(12)
                .WithMaxRelativeError(0.05);

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
                            payloadSize?.Bytes,
                            measuredEvidenceId));
                })
                .ToArray();
            var reportedResults = CaptureReportedResults(
                runLibraries,
                environment.Id,
                reportedEvidenceId);
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
            var evidence = new List<BenchmarkEvidence>();
            if (measuredResults.Length > 0)
            {
                evidence.Add(CaptureEvidence(
                    options.EvidenceDirectory,
                    artifactsDirectory,
                    measuredEvidenceId,
                    "measured",
                    jobId,
                    environment,
                    runLibraries,
                    options.Smoke));
            }

            if (reportedResults.Length > 0)
            {
                evidence.Add(CreateEvidence(
                    reportedEvidenceId,
                    "reported",
                    jobId,
                    environment.Id,
                    null));
            }

            var report = new BenchmarkReport(
                2,
                module.Id,
                DateTimeOffset.UtcNow,
                [environment],
                benchmarkLibraries,
                features,
                evidence);
            if (existing is not null)
            {
                report = Merge(existing, report, runLibraries);
            }

            reportStore.Write(outputPath, report);
            PruneEvidence(options.EvidenceDirectory, report.Evidence ?? []);
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
        var evidenceIds = features
            .SelectMany(feature => feature.Results)
            .Select(result => result.EvidenceId)
            .Where(id => id is not null)
            .ToHashSet(StringComparer.Ordinal);
        var evidence = (existing.Evidence ?? [])
            .Concat(current.Evidence ?? [])
            .Where(item => evidenceIds.Contains(item.Id))
            .DistinctBy(item => item.Id, StringComparer.Ordinal)
            .OrderBy(item => item.GeneratedAtUtc)
            .ToArray();
        return current with
        {
            Environments = environments,
            Libraries = libraries,
            Features = features,
            Evidence = evidence
        };
    }

    private CapturedBenchmarkResult[] CaptureReportedResults(
        IReadOnlyList<MatrixLibrary> libraries,
        string environmentId,
        string evidenceId)
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
                    item.Method.GetCustomAttribute<PayloadSizeAttribute>()?.Bytes,
                    evidenceId)))
            .ToArray();
    }

    private BenchmarkEvidence CaptureEvidence(
        string? evidenceRoot,
        string artifactsDirectory,
        string evidenceId,
        string kind,
        string job,
        BenchmarkEnvironment environment,
        IReadOnlyList<MatrixLibrary> libraries,
        bool smoke)
    {
        if (string.IsNullOrWhiteSpace(evidenceRoot))
        {
            return CreateEvidence(evidenceId, kind, job, environment.Id, null);
        }

        var evidenceDirectory = Path.Combine(Path.GetFullPath(evidenceRoot), evidenceId);
        var rawDirectory = Path.Combine(evidenceDirectory, "benchmarkdotnet");
        Directory.CreateDirectory(evidenceDirectory);
        CopyEvidenceFiles(artifactsDirectory, rawDirectory);
        File.WriteAllText(
            Path.Combine(evidenceDirectory, "environment.json"),
            jsonSerializer.Serialize(environment, EvidenceJsonOptions) + Environment.NewLine);
        File.WriteAllText(
            Path.Combine(evidenceDirectory, "command.txt"),
            $"dotnet run --project src/{moduleAssembly.Value.GetName().Name} --configuration Release "
            + $"-p:MatrixMode=Benchmark -- --output reports/{module.ReportDirectory}/benchmarks.json"
            + $" --libraries \"{string.Join(',', libraries.Select(library => library.Id))}\""
            + (smoke ? " --smoke" : string.Empty)
            + Environment.NewLine);

        var files = Directory
            .EnumerateFiles(evidenceDirectory, "*", SearchOption.AllDirectories)
            .Select(path => new
            {
                path = Path.GetRelativePath(evidenceDirectory, path).Replace('\\', '/'),
                sha256 = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant(),
                size = new FileInfo(path).Length
            })
            .OrderBy(file => file.path, StringComparer.Ordinal)
            .ToArray();
        var evidence = CreateEvidence(
            evidenceId,
            kind,
            job,
            environment.Id,
            $"reports/{module.ReportDirectory}/evidence/{evidenceId}/manifest.json");
        File.WriteAllText(
            Path.Combine(evidenceDirectory, "manifest.json"),
            jsonSerializer.Serialize(new
            {
                schemaVersion = 1,
                evidence,
                files
            }, EvidenceJsonOptions) + Environment.NewLine);
        return evidence;
    }

    private static BenchmarkEvidence CreateEvidence(
        string id,
        string kind,
        string job,
        string environmentId,
        string? manifestPath)
    {
        var server = Environment.GetEnvironmentVariable("GITHUB_SERVER_URL");
        var repository = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
        var runId = Environment.GetEnvironmentVariable("GITHUB_RUN_ID");
        var workflowRunUrl = server is not null && repository is not null && runId is not null
            ? $"{server}/{repository}/actions/runs/{runId}"
            : null;
        return new BenchmarkEvidence(
            id,
            kind,
            DateTimeOffset.UtcNow,
            Environment.GetEnvironmentVariable("GITHUB_SHA"),
            workflowRunUrl,
            job,
            environmentId,
            "matrix-reports",
            manifestPath,
            repository);
    }

    private static string CreateEvidenceId(string moduleId)
    {
        var runId = Environment.GetEnvironmentVariable("GITHUB_RUN_ID");
        var attempt = Environment.GetEnvironmentVariable("GITHUB_RUN_ATTEMPT");
        var run = string.IsNullOrWhiteSpace(runId)
            ? $"{DateTimeOffset.UtcNow:yyyyMMddTHHmmssZ}-{Guid.NewGuid():N}"[..32]
            : $"{runId}-{attempt ?? "1"}";
        return $"{run}-{moduleId}";
    }

    private static void CopyEvidenceFiles(string source, string destination)
    {
        Directory.CreateDirectory(destination);
        foreach (var file in Directory
                     .EnumerateFiles(source, "*", SearchOption.AllDirectories)
                     .Where(IsEvidenceFile))
        {
            var target = Path.Combine(destination, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Copy(file, target, true);
        }
    }

    private static bool IsEvidenceFile(string path) =>
        path.EndsWith("-report-full.json", StringComparison.OrdinalIgnoreCase)
        || Path.GetExtension(path).Equals(".log", StringComparison.OrdinalIgnoreCase);

    private static void PruneEvidence(
        string? evidenceRoot,
        IReadOnlyList<BenchmarkEvidence> evidence)
    {
        if (string.IsNullOrWhiteSpace(evidenceRoot) || !Directory.Exists(evidenceRoot))
        {
            return;
        }

        var activeIds = evidence
            .Where(item => item.ManifestPath is not null)
            .Select(item => item.Id)
            .ToHashSet(StringComparer.Ordinal);
        foreach (var directory in Directory.EnumerateDirectories(evidenceRoot))
        {
            if (!activeIds.Contains(Path.GetFileName(directory)))
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
