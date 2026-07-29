namespace Build.Targets;

internal interface IReportChartsTarget
{
    int Run(IReadOnlyList<DiscoveredMatrixModule> modules);
}
