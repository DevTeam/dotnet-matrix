namespace Build.Targets;

internal interface IMatrixModuleDiscovery
{
    IReadOnlyList<DiscoveredMatrixModule> Discover();
}
