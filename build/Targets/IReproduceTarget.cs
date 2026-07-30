namespace Build.Targets;

internal interface IReproduceTarget
{
    Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        bool skipBenchmarks,
        bool launchBrowser,
        CancellationToken cancellationToken);
}
