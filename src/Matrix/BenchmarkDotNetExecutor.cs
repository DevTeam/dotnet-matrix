using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;
using System.Reflection;

namespace Matrix;

public sealed class BenchmarkDotNetExecutor(IBenchmarkEnvironmentProvider environmentProvider) : IBenchmarkExecutor
{
    public BenchmarkExecution Execute(
        Assembly moduleAssembly,
        IReadOnlySet<string> libraryIds,
        bool smoke,
        string artifactsDirectory)
    {
        var jobId = smoke ? "Smoke" : "Quick";
        var job = Job.Default
            .WithId(jobId)
            .WithArguments([new MsBuildArgument("/p:MatrixMode=Benchmark")]);
        // Preserve roughly the old 2 + 5 iterations at 500 ms time budget,
        // but collect more samples and let BenchmarkDotNet adapt to noisy cases.
        job = smoke
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

        var config = ManualConfig.Create(DefaultConfig.Instance)
            .AddFilter(new MatrixBenchmarkFilter(libraryIds))
            .AddJob(job)
            .AddExporter(JsonExporter.Full)
            .WithArtifactsPath(artifactsDirectory);
        var summaries = BenchmarkSwitcher
            .FromAssembly(moduleAssembly)
            .Run(["--filter", "*"], config)
            .ToList();

        var resolvedJobs = summaries
            .SelectMany(summary => summary.BenchmarksCases)
            .Select(benchmarkCase => benchmarkCase.Job)
            .ToArray();
        var resolvedJobIds = resolvedJobs
            .Select(resolvedJob => resolvedJob.ResolvedId)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (resolvedJobIds.Length > 1)
        {
            throw new InvalidOperationException(
                $"ERROR: the benchmark configuration resolved to {resolvedJobIds.Length} jobs "
                + $"({string.Join(", ", resolvedJobIds)}). Environment capture below records "
                + "one BenchmarkEnvironment per run, taken from the first resolved job, so "
                + "results from the other job(s) would be tagged with the wrong environment. "
                + "Refusing to write a report.");
        }

        var environment = environmentProvider.Capture(
            "BenchmarkDotNet",
            typeof(BenchmarkSwitcher).Assembly,
            jobId,
            Describe(resolvedJobs.FirstOrDefault() ?? job));
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
        return new BenchmarkExecution(environment, measuredResults);
    }

    private static BenchmarkJobEnvironment Describe(Job job) => new(
        job.Environment.HasValue(EnvironmentMode.RuntimeCharacteristic) ? job.Environment.Runtime?.Name : null,
        job.Environment.HasValue(EnvironmentMode.PlatformCharacteristic) ? job.Environment.Platform.ToString() : null,
        job.Environment.Gc.HasValue(GcMode.ServerCharacteristic) ? job.Environment.Gc.Server : null);
}
