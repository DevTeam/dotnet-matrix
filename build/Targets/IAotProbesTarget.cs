namespace Build.Targets;

internal interface IAotProbesTarget
{
    Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CancellationToken cancellationToken,
        string? category = null,
        string? libraries = null);
}
