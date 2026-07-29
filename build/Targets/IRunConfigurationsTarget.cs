namespace Build.Targets;

internal interface IRunConfigurationsTarget
{
    int Run(IReadOnlyList<DiscoveredMatrixModule> modules);
}
