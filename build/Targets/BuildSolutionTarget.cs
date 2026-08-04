using HostCommandLine = HostApi.CommandLine;

namespace Build.Targets;

internal sealed class BuildSolutionTarget(
    IBuildPaths buildPaths,
    IQuietProcessRunner processRunner) : IBuildSolutionTarget
{
    private const string Operation = "Build solution";

    public async Task<int> RunAsync(CancellationToken cancellationToken)
    {
        Info($"{Operation}: dotnet-matrix.slnx");
        var result = await processRunner.RunAsync(
            new HostCommandLine(
                "dotnet",
                buildPaths.SolutionDirectory,
                ["build", "dotnet-matrix.slnx"],
                [],
                Operation),
            Operation,
            cancellationToken);
        if (result == 0)
        {
            Info($"{Operation} completed.");
        }

        return result;
    }
}
