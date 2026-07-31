using Matrix;
using System.Text;

namespace Build.Targets;

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
        var selectedModules = SelectModules(modules, options.Category);
        if (selectedModules.Count == 0)
        {
            await Console.Error.WriteLineAsync(
                $"Unknown matrix category '{options.Category}'. "
                + $"Available categories: {string.Join(", ", modules.Select(module => module.Metadata.Id))}.");
            return 1;
        }

        var output = ResolveOutputDirectory(options.OutputDirectory);
        RecreateDirectory(output);

        var summary = new StringBuilder()
            .AppendLine("## .NET Matrix reports")
            .AppendLine()
            .AppendLine($"- Category: `{options.Category ?? "*"}`")
            .AppendLine($"- Libraries: `{options.Libraries ?? "*"}`")
            .AppendLine(
                options.Smoke
                    ? "- Benchmark job: `Smoke` - one iteration only, the numbers are not trustworthy"
                    : "- Benchmark job: `Quick`");

        var exitCode = 0;
        var validationFailed = false;
        foreach (var module in selectedModules)
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
            foreach (var module in selectedModules)
            {
                var result = await matrixTarget.RunAsync(
                    module,
                    MatrixMode.Benchmark,
                    options.Libraries,
                    options.Smoke,
                    cancellationToken,
                    Path.Combine(
                        output,
                        "reports",
                        module.Metadata.ReportDirectory,
                        "evidence"));
                exitCode = Combine(exitCode, result);
                AppendBenchmarks(summary, module, result);
            }
        }

        Stage(selectedModules, output, benchmarked);
        WriteSummary(output, options.Category ?? "all", summary);
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

            if (benchmarked)
            {
                StageEvidence(module, directory);
            }
        }
    }

    private void StageEvidence(DiscoveredMatrixModule module, string reportDestination)
    {
        var report = reportStore.Read<BenchmarkReport>(ReportPath(module, BenchmarksFileName));
        var activeIds = (report?.Evidence ?? [])
            .Where(evidence => evidence.ManifestPath is not null)
            .Select(evidence => evidence.Id)
            .ToHashSet(StringComparer.Ordinal);
        var sourceRoot = Path.Combine(
            buildPaths.SolutionDirectory,
            "reports",
            module.Metadata.ReportDirectory,
            "evidence");
        var destinationRoot = Path.Combine(reportDestination, "evidence");
        Directory.CreateDirectory(destinationRoot);

        foreach (var evidenceId in activeIds)
        {
            var source = Path.Combine(sourceRoot, evidenceId);
            var destination = Path.Combine(destinationRoot, evidenceId);
            if (Directory.Exists(source) && !Directory.Exists(destination))
            {
                CopyDirectory(source, destination);
            }
        }

        foreach (var directory in Directory.EnumerateDirectories(destinationRoot))
        {
            if (!activeIds.Contains(Path.GetFileName(directory)))
            {
                Directory.Delete(directory, true);
            }
        }
    }

    private static void CopyDirectory(string source, string destination)
    {
        foreach (var file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            var target = Path.Combine(destination, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Copy(file, target, true);
        }
    }

    private static void WriteSummary(
        string output,
        string category,
        StringBuilder summary)
    {
        var directory = Path.Combine(output, "summaries");
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, $"{category}.md");
        File.WriteAllText(path, summary.ToString());
        Console.WriteLine($"Report summary: {path}");
    }

    private static IReadOnlyList<DiscoveredMatrixModule> SelectModules(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        string? category) =>
        string.IsNullOrWhiteSpace(category)
            ? modules
            : modules
                .Where(module => module.Metadata.Id.Equals(
                    category,
                    StringComparison.OrdinalIgnoreCase))
                .ToArray();

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
