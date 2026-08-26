using System.Text.Json;
using System.Text.RegularExpressions;
using Matrix;
// ReSharper disable InvertIf

namespace Build.Targets;

internal sealed partial class MetadataTarget(
    IBuildPaths buildPaths,
    IJsonSerializer jsonSerializer) : IMetadataTarget
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };

    private static readonly JsonSerializerOptions ReadOptions =
        new(JsonSerializerDefaults.Web);

    public int Run(IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        foreach (var module in modules)
        {
            var directory = Path.Combine(
                buildPaths.SolutionDirectory,
                "metadata",
                module.Metadata.ReportDirectory);
            Directory.CreateDirectory(directory);
            foreach (var library in module.Metadata.LibraryMetadata.Libraries)
            {
                var logoPath = Path.Combine(directory, library.Logo);
                if (!File.Exists(logoPath))
                {
                    Console.Error.WriteLine(
                        $"Metadata logo for '{library.Id}' does not exist: {logoPath}");
                    return 1;
                }
            }

            var path = Path.Combine(directory, "libraries.json");
            File.WriteAllText(
                path,
                jsonSerializer.Serialize(module.Metadata.LibraryMetadata, JsonOptions)
                + Environment.NewLine);
            var featurePath = Path.Combine(directory, "features.json");
            File.WriteAllText(
                featurePath,
                jsonSerializer.Serialize(module.Metadata.FeatureMetadata, JsonOptions)
                + Environment.NewLine);
            if (Validate(module, directory) != 0)
            {
                return 1;
            }
        }

        Info($"Metadata: {modules.Count} categories generated.");
        return 0;
    }

    /// <summary>
    /// The declared features, the chart groups and the feature contract are three
    /// files that have to agree, and nothing kept them in agreement. A contract
    /// claimed a scenario was excluded from the rating, which no code implements;
    /// another scenario had no contract section at all. Both survived for as long
    /// as they did because only a reader could notice them.
    /// </summary>
    private int Validate(DiscoveredMatrixModule module, string metadataDirectory)
    {
        var features = module.Metadata.FeatureMetadata.Features;
        var chartsPath = Path.Combine(metadataDirectory, "charts.json");
        if (!File.Exists(chartsPath))
        {
            Console.Error.WriteLine(
                $"{module.Metadata.Name}: chart groups are missing: {chartsPath}");
            return 1;
        }

        var charts = jsonSerializer.Deserialize<MatrixChartCatalog>(
            File.ReadAllText(chartsPath),
            ReadOptions);
        if (charts is null)
        {
            Console.Error.WriteLine(
                $"{module.Metadata.Name}: chart groups cannot be read: {chartsPath}");
            return 1;
        }

        var errors = new List<string>();
        var featureIds = new HashSet<string>(
            features.Select(feature => feature.Id),
            StringComparer.OrdinalIgnoreCase);
        var groupIds = new HashSet<string>(
            charts.Groups.Select(group => group.Id),
            StringComparer.OrdinalIgnoreCase);

        // charts.json is written by hand while features.json is generated, so the
        // two drift silently. A group may only draw a declared feature, and a
        // feature may not be drawn twice.
        var placements = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var group in charts.Groups)
        {
            foreach (var feature in group.Features)
            {
                if (!featureIds.Contains(feature))
                {
                    errors.Add(
                        $"chart group '{group.Id}' draws unknown feature '{feature}'.");
                }

                placements[feature] = placements.GetValueOrDefault(feature) + 1;
            }
        }

        foreach (var placement in placements.Where(item => item.Value > 1))
        {
            errors.Add(
                $"feature '{placement.Key}' is drawn in {placement.Value} chart groups; "
                + "a feature belongs to at most one.");
        }

        var contractPath = ContractPath(module);
        if (!File.Exists(contractPath))
        {
            errors.Add($"feature contract is missing: {contractPath}");
        }
        else
        {
            // A floor rather than a ceiling: it catches a scenario nobody wrote a
            // contract for, which is the failure that happened, without imposing a
            // single layout on contracts that do not share one.
            var contract = File.ReadAllText(contractPath);
            foreach (var feature in features)
            {
                if (!contract.Contains(feature.Name, StringComparison.Ordinal))
                {
                    errors.Add(
                        $"feature contract does not document '{feature.Name}': {contractPath}");
                }
            }

            // `Group:` names the chart group a scenario is drawn in and nothing
            // else. It was called `Rating:` under the medal rule, and values such
            // as `feature-only` outlived it, reading as though a scenario could
            // opt out of the rating. See workflows/rating.md.
            foreach (var match in GroupFieldRegex().Matches(contract).Cast<Match>())
            {
                var id = match.Groups[1].Value;
                if (!groupIds.Contains(id))
                {
                    errors.Add(
                        $"feature contract names chart group '{id}', "
                        + $"which {chartsPath} does not define.");
                }
            }
        }

        foreach (var error in errors)
        {
            Console.Error.WriteLine($"{module.Metadata.Name}: {error}");
        }

        if (errors.Count != 0)
        {
            return 1;
        }

        // Not an error: a chart group is a curated view, not a partition. It is
        // reported because such a scenario is rated but appears in no overview
        // chart, and that is easy to arrive at by accident. A feature marked
        // Rated: false is excluded from this check — it belongs in no group by
        // design, not by omission.
        var undrawn = features
            .Where(feature => feature.Rated && !placements.ContainsKey(feature.Id))
            .Select(feature => feature.Id)
            .ToArray();
        if (undrawn.Length != 0)
        {
            Info(
                $"{module.Metadata.Name}: rated but drawn in no overview group: "
                + string.Join(", ", undrawn) + ".");
        }

        return 0;
    }

    /// <summary>
    /// `ReportDirectory` is Pascal case and the contracts are kebab case:
    /// `CsvProcessing` is documented by `csv-processing.md`.
    /// </summary>
    private string ContractPath(DiscoveredMatrixModule module) =>
        Path.Combine(
            buildPaths.SolutionDirectory,
            "workflows",
            "feature-contracts",
            KebabRegex().Replace(module.Metadata.ReportDirectory, "-$1").ToLowerInvariant()
            + ".md");

    [GeneratedRegex(@"(?m)^- Group: `?([A-Za-z][A-Za-z0-9-]*)`?\.?\s*$")]
    private static partial Regex GroupFieldRegex();

    [GeneratedRegex("(?<!^)([A-Z])")]
    private static partial Regex KebabRegex();
}
