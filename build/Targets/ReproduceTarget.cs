namespace Build.Targets;

internal sealed class ReproduceTarget(
    IPrepareCommitTarget prepareCommitTarget,
    ILocalWebTarget localWebTarget) : IReproduceTarget
{
    public async Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        bool skipBenchmarks,
        bool launchBrowser,
        CancellationToken cancellationToken)
    {
        Info(skipBenchmarks
            ? "Reproducing the application from reports already on disk."
            : "Reproducing all validation and benchmark results.");
        var result = await prepareCommitTarget.RunAsync(
            modules,
            !skipBenchmarks,
            cancellationToken);
        if (result != 0)
        {
            return result;
        }

        Info("Reports, charts, metadata, README, and run configurations are ready.");
        return await localWebTarget.RunAsync(launchBrowser, cancellationToken);
    }
}
