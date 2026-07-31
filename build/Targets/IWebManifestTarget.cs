namespace Build.Targets;

internal interface IWebManifestTarget
{
    int Run(IReadOnlyList<DiscoveredMatrixModule> modules);
}
