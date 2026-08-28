using Matrix;
using System.Text;
using System.Text.RegularExpressions;
using HostCommandLine = HostApi.CommandLine;

namespace Build.Targets;

/// <summary>
/// Publishes one Native AOT probe per library and records the outcome as a compatibility feature
/// in the module's feature report.
/// </summary>
/// <remarks>
/// This is status-only data. It carries no timing, it is written to <c>features.json</c> and never
/// to <c>benchmarks.json</c>, and the rating is computed from benchmark reports alone, so nothing
/// here can move a score. Run it after feature validation, which rewrites <c>features.json</c>,
/// and before reports are staged.
/// </remarks>
internal sealed partial class AotProbesTarget(
    IBuildPaths buildPaths,
    IMatrixReportStore reportStore,
    IQuietProcessRunner processRunner) : IAotProbesTarget
{
    /// <summary>
    /// The feature id and name written into the report.
    /// </summary>
    private const string FeatureId = "NativeAot";

    private const string FeatureName = "Native AOT";

    /// <summary>
    /// Native AOT is a deployment capability, not a scenario. See
    /// <see cref="MatrixFeatureOrders.Deployment"/> for what that order separates.
    /// </summary>
    private const int FeatureOrder = MatrixFeatureOrders.Deployment;

    private const string ProbeProjectSuffix = ".Aot";

    private const string RuntimeIdentifier = "linux-x64";

    private const string TargetFramework = "net10.0";

    public async Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CancellationToken cancellationToken)
    {
        var probed = 0;
        foreach (var module in modules)
        {
            var projectPath = ProbeProjectPath(module);
            if (projectPath is null)
            {
                continue;
            }

            var result = await ProbeModuleAsync(module, projectPath, cancellationToken);
            if (result != 0)
            {
                return result;
            }

            probed++;
        }

        Info(probed == 0
            ? "Native AOT: no module has a probe project, nothing to do."
            : $"Native AOT: {probed} module(s) probed.");
        return 0;
    }

    private async Task<int> ProbeModuleAsync(
        DiscoveredMatrixModule module,
        string projectPath,
        CancellationToken cancellationToken)
    {
        var reportPath = Path.Combine(
            buildPaths.SolutionDirectory,
            "reports",
            module.Metadata.ReportDirectory,
            "features.json");
        var report = reportStore.Read<FeatureReport>(reportPath);
        if (report is null)
        {
            Console.Error.WriteLine(
                $"ERROR: {reportPath} does not exist. Run feature validation for "
                + $"{module.Metadata.Name} before probing Native AOT, because validation rewrites "
                + "the report this target enriches.");
            return 1;
        }

        var probeVersion = ProbeVersion(projectPath);
        var results = new List<FeatureResult>(module.Metadata.Libraries.Count);
        foreach (var library in module.Metadata.Libraries)
        {
            results.Add(await ProbeLibraryAsync(
                projectPath,
                library,
                probeVersion,
                cancellationToken));
        }

        var entry = new FeatureReportEntry(
            FeatureOrder,
            FeatureId,
            FeatureName,
            results
                .OrderBy(result => result.LibraryId, StringComparer.OrdinalIgnoreCase)
                .ToArray());

        var features = report.Features
            .Where(existing => !existing.Id.Equals(FeatureId, StringComparison.Ordinal))
            .Append(entry)
            .OrderBy(feature => feature.Order)
            .ToArray();

        reportStore.Write(reportPath, report with { Features = features });
        Info($"Native AOT: {module.Metadata.Name} written to {reportPath}.");
        return 0;
    }

    private async Task<FeatureResult> ProbeLibraryAsync(
        string projectPath,
        MatrixLibrary library,
        string probeVersion,
        CancellationToken cancellationToken)
    {
        var probeName = ProbeName(library.Id);
        if (!File.Exists(Path.Combine(
                Path.GetDirectoryName(projectPath)!,
                "Probes",
                $"{probeName}.cs")))
        {
            return new FeatureResult(
                library.Id,
                nameof(FeatureStatus.NotApplicable),
                $"No Native AOT probe exists for {library.Id}.",
                0);
        }

        if (library.Package is null || library.Version is null)
        {
            return new FeatureResult(
                library.Id,
                nameof(FeatureStatus.NotApplicable),
                $"{library.Id} declares no package, so there is nothing to publish.",
                0);
        }

        var output = new StringBuilder();
        var publish = await processRunner.RunAsync(
            new HostCommandLine(
                "dotnet",
                buildPaths.SolutionDirectory,
                [
                    "publish",
                    projectPath,
                    "--configuration",
                    "Release",
                    "--runtime",
                    RuntimeIdentifier,
                    $"-p:MatrixAotLibrary={probeName}",
                    $"-p:MatrixAotPackage={library.Package}",
                    $"-p:MatrixAotPackageVersion={library.Version}"
                ],
                [],
                $"AOT publish {library.Id}"),
            $"AOT publish {library.Id}",
            cancellationToken,
            outputHandler: line => output.AppendLine(line.Line));

        var warnings = TrimWarnings(output.ToString());
        var note = Note(warnings, probeVersion);
        if (publish != 0)
        {
            return new FeatureResult(
                library.Id,
                nameof(FeatureStatus.Unsupported),
                $"Native AOT publish failed: {LastDiagnostic(output.ToString())}",
                0,
                note);
        }

        var executable = Path.Combine(
            Path.GetDirectoryName(projectPath)!,
            "bin",
            "Release",
            TargetFramework,
            RuntimeIdentifier,
            "publish",
            $"{Path.GetFileNameWithoutExtension(projectPath)}.{probeName}");

        var run = new StringBuilder();
        var exitCode = await processRunner.RunAsync(
            new HostCommandLine(
                executable,
                buildPaths.SolutionDirectory,
                [],
                [],
                $"AOT probe {library.Id}"),
            $"AOT probe {library.Id}",
            cancellationToken,
            outputHandler: line => run.AppendLine(line.Line));

        return exitCode == 0
            ? new FeatureResult(library.Id, nameof(FeatureStatus.Supported), null, 0, note)
            : new FeatureResult(
                library.Id,
                nameof(FeatureStatus.Failed),
                $"Native AOT probe failed: {LastDiagnostic(run.ToString())}",
                0,
                note);
    }

    /// <summary>
    /// A trim warning never decides support: a library can warn and still publish and run. It is
    /// recorded as a note so a reader can see the asymmetry between libraries.
    /// </summary>
    private static string? Note(int warnings, string probeVersion) =>
        warnings == 0
            ? $"0 trim warnings (probe v{probeVersion})"
            : $"{warnings} trim warning{(warnings == 1 ? string.Empty : "s")} (probe v{probeVersion})";

    /// <summary>
    /// Counts ILC diagnostics. The count depends on what the probe compiles, which is why it is
    /// always published beside the probe version that produced it.
    /// </summary>
    private static int TrimWarnings(string output) => IlcWarning().Matches(output).Count;

    private static string LastDiagnostic(string output)
    {
        var lines = output
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(line => line.Length > 0)
            .ToArray();
        return lines.Length == 0 ? "no diagnostic was captured" : lines[^1];
    }

    /// <summary>
    /// Derives the probe source name from the library id, so that adding a library needs a probe
    /// file and nothing else. Only letters and digits survive, which turns
    /// <c>Microsoft.Extensions.Logging</c> into <c>MicrosoftExtensionsLogging</c> and leaves
    /// <c>log4net</c> alone.
    /// </summary>
    private static string ProbeName(string libraryId) =>
        new(libraryId.Where(char.IsLetterOrDigit).ToArray());

    private string? ProbeProjectPath(DiscoveredMatrixModule module)
    {
        var directory = Path.GetDirectoryName(module.ProjectPath)!;
        var name = Path.GetFileNameWithoutExtension(module.ProjectPath) + ProbeProjectSuffix;
        var path = Path.Combine(
            Path.GetDirectoryName(directory)!,
            name,
            $"{name}.csproj");
        return File.Exists(path) ? path : null;
    }

    private static string ProbeVersion(string projectPath)
    {
        var match = ProbeVersionElement().Match(File.ReadAllText(projectPath));
        return match.Success ? match.Groups[1].Value : "unknown";
    }

    [GeneratedRegex(@"^ILC : ", RegexOptions.Multiline)]
    private static partial Regex IlcWarning();

    [GeneratedRegex(@"<MatrixAotProbeVersion>([^<]+)</MatrixAotProbeVersion>")]
    private static partial Regex ProbeVersionElement();
}
