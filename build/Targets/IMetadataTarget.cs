namespace Build.Targets;

internal interface IMetadataTarget
{
    int Run(IReadOnlyList<DiscoveredMatrixModule> modules);
}
