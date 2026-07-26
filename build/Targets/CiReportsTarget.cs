using Matrix;
using System.Text;

namespace Build.Targets;

internal sealed record CiReportsOptions(
    string? Libraries,
    bool Smoke,
    bool SkipBenchmarks,
    string? OutputDirectory);

internal interface ICiReportsTarget
{
    Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CiReportsOptions options,
        CancellationToken cancellationToken);
}

internal sealed class CiReportsTarget(
    IBuildPaths buildPaths,
    IMatrixTarget matrixTarget,
    IMatrixReportStore reportStore) : ICiReportsTarget
{
    private const string FeaturesFileName = "features.json";
    private const string BenchmarksFileName = "benchmarks.json";
    private const string FailedStatus = "Failed";

    public async Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CiReportsOptions options,
        CancellationToken cancellationToken)
    {
        var output = ResolveOutputDirectory(options.OutputDirectory);
        RecreateDirectory(output);

        var summary = new StringBuilder()
            .AppendLine("## .NET Matrix reports")
            .AppendLine()
            .AppendLine($"- Libraries: `{options.Libraries ?? "*"}`")
            .AppendLine(
                options.Smoke
                    ? "- Benchmark job: `Smoke` - one iteration only, the numbers are not trustworthy"
                    : "- Benchmark job: `Quick`");

        var exitCode = 0;
        var validationFailed = false;
        foreach (var module in modules)
        {
            var result = await matrixTarget.RunAsync(
                module,
                MatrixMode.Validation,
                options.Libraries,
                false,
                cancellationToken);
            validationFailed |= result != 0;
            exitCode = Combine(exitCode, result);
            AppendFeatures(summary, module, result);
        }

        var benchmarked = !validationFailed && !options.SkipBenchmarks;
        if (validationFailed)
        {
            summary
                .AppendLine()
                .AppendLine(
                    "> Benchmarks were skipped because feature validation failed. "
                    + "Only the feature report is published.");
            await Console.Error.WriteLineAsync(
                "Feature validation failed, skipping benchmarks. Only the feature report is staged.");
        }
        else if (options.SkipBenchmarks)
        {
            summary
                .AppendLine()
                .AppendLine("> Benchmarks were skipped by request (`--skip-benchmarks`).");
        }
        else
        {
            foreach (var module in modules)
            {
                var result = await matrixTarget.RunAsync(
                    module,
                    MatrixMode.Benchmark,
                    options.Libraries,
                    options.Smoke,
                    cancellationToken);
                exitCode = Combine(exitCode, result);
                AppendBenchmarks(summary, module, result);
            }
        }

        Stage(modules, output, benchmarked);
        WriteSummary(output, summary);
        Console.WriteLine($"Report artifact: {output}");
        return exitCode;
    }

    private void AppendFeatures(
        StringBuilder summary,
        DiscoveredMatrixModule module,
        int exitCode)
    {
        summary
            .AppendLine()
            .AppendLine($"### {module.Metadata.Name} features")
            .AppendLine()
            .AppendLine($"- Exit code: `{exitCode}`");

        var report = reportStore.Read<FeatureReport>(ReportPath(module, FeaturesFileName));
        if (report?.Features is null)
        {
            summary.AppendLine("- No feature report was produced.");
            return;
        }

        var results = report.Features
            .SelectMany(feature => feature.Results.Select(result => (feature, result)))
            .ToArray();
        var statuses = results
            .GroupBy(item => item.result.Status, StringComparer.OrdinalIgnoreCase)
            .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
            .Select(group => $"{group.Key}: {group.Count()}");
        summary
            .AppendLine($"- Libraries: {report.Libraries.Count}")
            .AppendLine($"- Results: {string.Join(", ", statuses)}");

        foreach (var (feature, result) in results.Where(item =>
                     item.result.Status.Equals(FailedStatus, StringComparison.OrdinalIgnoreCase)))
        {
            summary.AppendLine(
                $"- Failed: `{result.LibraryId}` / `{feature.Id}` - {result.Reason}");
        }
    }

    private void AppendBenchmarks(
        StringBuilder summary,
        DiscoveredMatrixModule module,
        int exitCode)
    {
        summary
            .AppendLine()
            .AppendLine($"### {module.Metadata.Name} benchmarks")
            .AppendLine()
            .AppendLine($"- Exit code: `{exitCode}`");

        var report = reportStore.Read<BenchmarkReport>(ReportPath(module, BenchmarksFileName));
        if (report?.Features is null)
        {
            summary.AppendLine("- No benchmark report was produced.");
            return;
        }

        var results = report.Features.SelectMany(feature => feature.Results).ToArray();
        summary
            .AppendLine($"- Libraries: {report.Libraries.Count}")
            .AppendLine($"- Results: {results.Count(result => result.Successful)} of {results.Length} successful");
        foreach (var environment in report.Environments ?? [])
        {
            summary.AppendLine(
                $"- Environment `{environment.Id}`: {environment.Job} job, "
                + $"{environment.OperatingSystem}, {environment.Processor} "
                + $"({environment.LogicalCoreCount} cores), SDK {environment.DotNetSdkVersion}");
        }
    }

    private void Stage(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        string output,
        bool benchmarked)
    {
        var fileNames = benchmarked
            ? new[] { FeaturesFileName, BenchmarksFileName }
            : [FeaturesFileName];
        foreach (var module in modules)
        {
            var directory = Path.Combine(output, "reports", module.Metadata.ReportDirectory);
            Directory.CreateDirectory(directory);
            foreach (var fileName in fileNames)
            {
                var source = ReportPath(module, fileName);
                if (!File.Exists(source))
                {
                    Console.Error.WriteLine($"WARNING: '{source}' does not exist and is not staged.");
                    continue;
                }

                var destination = Path.Combine(directory, fileName);
                File.Copy(source, destination, true);
                Console.WriteLine($"Staged: {destination}");
            }
        }
    }

    private static void WriteSummary(string output, StringBuilder summary)
    {
        var path = $"{output}-summary.md";
        File.WriteAllText(path, summary.ToString());
        Console.WriteLine($"Report summary: {path}");
    }

    private string ReportPath(DiscoveredMatrixModule module, string fileName) =>
        Path.Combine(
            buildPaths.SolutionDirectory,
            "reports",
            module.Metadata.ReportDirectory,
            fileName);

    private string ResolveOutputDirectory(string? outputDirectory) =>
        Path.GetFullPath(
            outputDirectory ?? Path.Combine("artifacts", "ci-reports"),
            buildPaths.SolutionDirectory);

    private static void RecreateDirectory(string directory)
    {
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, true);
        }

        Directory.CreateDirectory(directory);
    }

    private static int Combine(int exitCode, int result) =>
        exitCode != 0 ? exitCode : result;
}
