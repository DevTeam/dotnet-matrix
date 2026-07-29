namespace Build.Targets;

internal interface IPrepareCommitTarget
{
    Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        bool runMatrix,
        CancellationToken cancellationToken);
}
