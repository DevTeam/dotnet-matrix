namespace Build.Targets;

internal interface ICiReportsTarget
{
    Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CiReportsOptions options,
        CancellationToken cancellationToken);
}
