using Matrix;

namespace Build.Targets;

/// <summary>
/// Reports the modules whose feature report no longer carries the deployment capability their
/// Native AOT probe project publishes.
/// </summary>
/// <remarks>
/// Feature validation rewrites <c>features.json</c> from the scenarios it discovers, and a
/// deployment capability is not one of them, so the rewrite drops it. Only
/// <see cref="AotProbesTarget"/> puts it back, and it runs after validation — in a CI report
/// run and in <c>prepare-commit</c>. Any other path that validates a module and then publishes
/// its report (a single-module <c>*-validate</c> command, <c>finalize-commit</c>) leaves the
/// Deployment tab with nothing to show, silently: both the tab and the badge read only entries
/// flagged <see cref="FeatureReportEntry.IsDeployment"/>, and an empty set is indistinguishable
/// from a category that has no deployment capability at all.
/// </remarks>
internal sealed class DeploymentReportGuard(IBuildPaths buildPaths, IMatrixReportReader reportStore)
{
    /// <summary>
    /// The modules that are expected to publish a deployment capability but do not.
    /// </summary>
    /// <param name="modules">Modules to check.</param>
    /// <returns>Their names, in the order given; empty when nothing was lost.</returns>
    public IReadOnlyList<string> FindLostCapabilities(IEnumerable<DiscoveredMatrixModule> modules) =>
        modules
            .Where(module => HasProbeProject(module) && !HasCapability(module))
            .Select(module => module.Metadata.Name)
            .ToArray();

    /// <summary>
    /// Whether the module has a probe project, which is what makes a deployment capability
    /// expected of it. Derived from the repository rather than from a stored list, so a module
    /// added with a probe project is covered without registering anything.
    /// </summary>
    private static bool HasProbeProject(DiscoveredMatrixModule module) =>
        File.Exists(ProbeProjectPath(module));

    private bool HasCapability(DiscoveredMatrixModule module)
    {
        var report = reportStore.Read<FeatureReport>(ReportPath(module));
        return report?.Features is { } features && features.Any(feature => feature.IsDeployment);
    }

    private string ReportPath(DiscoveredMatrixModule module) =>
        Path.Combine(
            buildPaths.SolutionDirectory,
            "reports",
            module.Metadata.ReportDirectory,
            "features.json");

    /// <summary>
    /// The same location <see cref="AotProbesTarget"/> probes: a sibling project named after the
    /// module's own project with an <c>.Aot</c> suffix.
    /// </summary>
    private static string ProbeProjectPath(DiscoveredMatrixModule module)
    {
        var directory = Path.GetDirectoryName(module.ProjectPath)!;
        var name = Path.GetFileNameWithoutExtension(module.ProjectPath) + ".Aot";
        return Path.Combine(
            Path.GetDirectoryName(directory)!,
            name,
            $"{name}.csproj");
    }
}
