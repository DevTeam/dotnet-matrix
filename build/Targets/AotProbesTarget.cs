using Matrix;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
    /// Native AOT has its own order sequence, independent of scenario order: see
    /// <see cref="FeatureReportEntry.IsDeployment"/> for what separates the two. It is currently
    /// the only deployment capability the matrix records.
    /// </summary>
    private const int FeatureOrder = 1;

    private const string ProbeProjectSuffix = ".Aot";

    private const string TargetFramework = "net10.0";

    /// <summary>
    /// Native AOT has no cross-OS publish, so the probe always targets the machine it runs on.
    /// </summary>
    private static string RuntimeIdentifier => RuntimeInformation.RuntimeIdentifier;

    private static string ExecutableSuffix => OperatingSystem.IsWindows() ? ".exe" : string.Empty;

    public async Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CancellationToken cancellationToken,
        string? category = null,
        string? libraries = null)
    {
        var selectedModules = string.IsNullOrWhiteSpace(category)
            ? modules
            : modules
                .Where(module => module.Metadata.Id.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        if (selectedModules.Count == 0)
        {
            Console.Error.WriteLine(
                $"Unknown matrix category '{category}'. "
                + $"Available categories: {string.Join(", ", modules.Select(module => module.Metadata.Id))}.");
            return 1;
        }

        var libraryFilter = ParseLibraryFilter(libraries);

        var probed = 0;
        foreach (var module in selectedModules)
        {
            var projectPath = ProbeProjectPath(module);
            if (projectPath is null)
            {
                continue;
            }

            var result = await ProbeModuleAsync(module, projectPath, libraryFilter, cancellationToken);
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
        IReadOnlySet<string>? libraryFilter,
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

        var librariesToProbe = libraryFilter is null
            ? module.Metadata.Libraries
            : module.Metadata.Libraries.Where(library => libraryFilter.Contains(library.Id)).ToArray();
        if (librariesToProbe.Count == 0)
        {
            Console.Error.WriteLine(
                $"ERROR: no library in {module.Metadata.Name} matches the --libraries filter.");
            return 1;
        }

        var results = new List<FeatureResult>(librariesToProbe.Count);
        foreach (var library in librariesToProbe)
        {
            results.Add(await ProbeLibraryAsync(module, projectPath, library, cancellationToken));
        }

        // A --libraries filter probes a subset, so the rest of the module's existing results are
        // kept rather than dropped - the same partial-merge behavior feature validation and
        // benchmarking already give every other report.
        var existing = report.Features
            .FirstOrDefault(feature => feature.Id.Equals(FeatureId, StringComparison.Ordinal));
        var mergedResults = (existing?.Results ?? [])
            .Where(result => results.All(updated =>
                !updated.LibraryId.Equals(result.LibraryId, StringComparison.OrdinalIgnoreCase)))
            .Concat(results)
            .OrderBy(result => result.LibraryId, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var entry = new FeatureReportEntry(
            FeatureOrder,
            FeatureId,
            FeatureName,
            mergedResults,
            IsDeployment: true);

        var features = report.Features
            .Where(existingEntry => !existingEntry.Id.Equals(FeatureId, StringComparison.Ordinal))
            .Append(entry)
            .OrderBy(feature => feature.Order)
            .ToArray();

        reportStore.Write(reportPath, report with { Features = features });
        Info($"Native AOT: {module.Metadata.Name} written to {reportPath}.");
        return 0;
    }

    private async Task<FeatureResult> ProbeLibraryAsync(
        DiscoveredMatrixModule module,
        string projectPath,
        MatrixLibrary library,
        CancellationToken cancellationToken)
    {
        var projectDirectory = Path.GetDirectoryName(projectPath)!;
        var probeName = ProbeName(library.Id);
        var probeFile = Path.Combine(projectDirectory, "Probes", $"{probeName}.cs");
        if (!File.Exists(probeFile))
        {
            return new FeatureResult(
                library.Id,
                nameof(FeatureStatus.NotApplicable),
                $"No Native AOT probe exists for {library.Id}.",
                0);
        }

        var probeVersion = ProbeVersion(projectDirectory, probeFile);
        var arguments = new List<string>
        {
            "publish",
            projectPath,
            "--configuration",
            "Release",
            "--runtime",
            RuntimeIdentifier,
            $"-p:MatrixAotLibrary={probeName}"
        };

        // A baseline with no package of its own (System.Text.Json, System.Linq, ...) publishes
        // straight from the BCL: nothing to add here.
        if (library.Package is not null && library.Version is not null)
        {
            arguments.Add($"-p:MatrixAotPackage={library.Package}");
            arguments.Add($"-p:MatrixAotPackageVersion={library.Version}");
        }

        if (library.Companions.Count > 0)
        {
            // "#" joins companions, not ";": the dotnet/MSBuild command line splits a -p: switch
            // on both ";" and "," before the value ever reaches the project, no escaping survives
            // that split, and only "#" does not collide with either.
            arguments.Add(
                "-p:MatrixAotCompanions="
                + string.Join('#', library.Companions.Select(package => $"{package.Id}|{package.Version}")));
        }

        var output = new StringBuilder();
        var publishOperation = $"{module.Metadata.Id} AOT publish {library.Id}";
        var publish = await processRunner.RunAsync(
            new HostCommandLine(
                "dotnet",
                buildPaths.SolutionDirectory,
                arguments,
                [],
                publishOperation),
            publishOperation,
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
            projectDirectory,
            "bin",
            "Release",
            TargetFramework,
            RuntimeIdentifier,
            "publish",
            $"{Path.GetFileNameWithoutExtension(projectPath)}.{probeName}{ExecutableSuffix}");

        var run = new StringBuilder();
        var runOperation = $"{module.Metadata.Id} AOT probe {library.Id}";
        var exitCode = await processRunner.RunAsync(
            new HostCommandLine(
                executable,
                buildPaths.SolutionDirectory,
                [],
                [],
                runOperation),
            runOperation,
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
            ? $"0 trim warnings (probe {probeVersion}, {RuntimeIdentifier})"
            : $"{warnings} trim warning{(warnings == 1 ? string.Empty : "s")} (probe {probeVersion}, {RuntimeIdentifier})";

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

    /// <summary>
    /// Derives the probe version from the content that actually determines a trim-warning count:
    /// the probe file itself plus the shared host. A short hash, computed here rather than kept as
    /// a hand-maintained project property, so it can never go stale and never needs remembering.
    /// </summary>
    private static string ProbeVersion(string projectDirectory, string probeFile)
    {
        var content = string.Concat(
            File.ReadAllText(probeFile),
            File.ReadAllText(Path.Combine(projectDirectory, "Program.cs")),
            File.ReadAllText(Path.Combine(projectDirectory, "AotProbeHost.cs")));
        return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(content)))[..8];
    }

    private static HashSet<string>? ParseLibraryFilter(string? libraries) =>
        string.IsNullOrWhiteSpace(libraries)
            ? null
            : libraries
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

    [GeneratedRegex(@"^ILC : ", RegexOptions.Multiline)]
    private static partial Regex IlcWarning();
}
