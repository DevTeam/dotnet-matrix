using Matrix;
using HostCommandLine = HostApi.CommandLine;

namespace Build.Targets;

internal sealed class MatrixTarget(
    IBuildPaths buildPaths,
    IQuietProcessRunner processRunner) : IMatrixTarget
{
    public async Task<int> RunAsync(
        DiscoveredMatrixModule module,
        MatrixMode mode,
        string? libraries,
        bool smoke,
        CancellationToken cancellationToken,
        string? evidenceDirectory = null)
    {
        var report = Path.Combine(
            buildPaths.SolutionDirectory,
            "reports",
            module.Metadata.ReportDirectory,
            mode == MatrixMode.Validation ? "features.json" : "benchmarks.json");
        Directory.CreateDirectory(Path.GetDirectoryName(report)!);

        var arguments = new List<string>
        {
            "run",
            "--project",
            module.ProjectPath,
            "--configuration",
            "Release",
            $"-p:MatrixMode={mode}",
            "--",
            "--output",
            report
        };
        if (!string.IsNullOrWhiteSpace(libraries))
        {
            arguments.Add("--libraries");
            arguments.Add(libraries);
        }

        if (smoke)
        {
            arguments.Add("--smoke");
        }

        var resolvedEvidenceDirectory = evidenceDirectory;
        if (mode == MatrixMode.Benchmark && string.IsNullOrWhiteSpace(resolvedEvidenceDirectory))
        {
            resolvedEvidenceDirectory = Path.Combine(
                buildPaths.SolutionDirectory,
                "reports",
                module.Metadata.ReportDirectory,
                "evidence");
        }

        if (!string.IsNullOrWhiteSpace(resolvedEvidenceDirectory))
        {
            arguments.Add("--evidence");
            arguments.Add(resolvedEvidenceDirectory);
        }

        var selection = libraries ?? "all libraries";
        var operation = $"{mode} {module.Metadata.Name}";
        Info($"{operation}: {selection}");
        var result = await processRunner.RunAsync(
            new HostCommandLine(
                "dotnet",
                buildPaths.SolutionDirectory,
                arguments,
                [],
                operation),
            operation,
            cancellationToken);
        if (result == 0)
        {
            Info($"{operation} completed.");
        }

        return result;
    }
}
