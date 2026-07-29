namespace Build.Targets;

internal interface ICiMatrixTarget
{
    int Run(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        string? outputFile);
}
