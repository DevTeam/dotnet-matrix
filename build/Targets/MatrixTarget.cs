using Matrix;
using System.Diagnostics;

namespace Build.Targets;

internal sealed class MatrixTarget(IBuildPaths buildPaths) : IMatrixTarget
{
    public async Task<int> RunAsync(
        DiscoveredMatrixModule module,
        MatrixMode mode,
        string? libraries,
        bool smoke,
        CancellationToken cancellationToken)
    {
        var report = Path.Combine(
            buildPaths.SolutionDirectory,
            "reports",
            module.Metadata.ReportDirectory,
            mode == MatrixMode.Validation ? "features.json" : "benchmarks.json");
        Directory.CreateDirectory(Path.GetDirectoryName(report)!);

        var startInfo = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = buildPaths.SolutionDirectory,
            UseShellExecute = false
        };
        startInfo.ArgumentList.Add("run");
        startInfo.ArgumentList.Add("--project");
        startInfo.ArgumentList.Add(module.ProjectPath);
        startInfo.ArgumentList.Add("--configuration");
        startInfo.ArgumentList.Add("Release");
        startInfo.ArgumentList.Add($"-p:MatrixMode={mode}");
        startInfo.ArgumentList.Add("--");
        startInfo.ArgumentList.Add("--output");
        startInfo.ArgumentList.Add(report);
        if (!string.IsNullOrWhiteSpace(libraries))
        {
            startInfo.ArgumentList.Add("--libraries");
            startInfo.ArgumentList.Add(libraries);
        }

        if (smoke)
        {
            startInfo.ArgumentList.Add("--smoke");
        }

        Console.WriteLine(
            $"Running {mode} for {module.Metadata.Name}: {libraries ?? "all libraries"}");
        using var process = Process.Start(startInfo)
                            ?? throw new InvalidOperationException("Could not start dotnet.");
        await process.WaitForExitAsync(cancellationToken);
        return process.ExitCode;
    }
}
