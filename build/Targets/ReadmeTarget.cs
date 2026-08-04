using Matrix;
using System.Text.Json;
using System.Text.RegularExpressions;
// ReSharper disable UseCollectionExpression

namespace Build.Targets;

internal sealed partial class ReadmeTarget(
    IBuildPaths buildPaths,
    IMetadataTarget metadataTarget,
    IReportChartsTarget reportChartsTarget,
    ITemplateEngine templateEngine) : IReadmeTarget
{
    private const string Template = "/Templates/Readme.cshtml";
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public async Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CancellationToken cancellationToken)
    {
        var metadataResult = metadataTarget.Run(modules);
        if (metadataResult != 0)
        {
            return metadataResult;
        }

        var result = reportChartsTarget.Run(modules);
        if (result != 0)
        {
            return result;
        }

        var categories = modules
            .Select(CreateCategory)
            .Where(category => category is not null)
            .Select(category => category!)
            .ToArray();
        // var catalog = Read<MatrixWebCatalog>(Path.Combine(buildPaths.SolutionDirectory, "src", "Matrix.Web", "wwwroot", "data", "catalog.json"));
        // var applicationUrl = $"https://{catalog.Repository.Owner.ToLowerInvariant()}.github.io/{catalog.Repository.Name}/";
        const string applicationUrl = "https://matrix.dev-team.org/";
        var model = new ReadmeModel(applicationUrl, categories);
        var path = Path.Combine(buildPaths.SolutionDirectory, "README.md");
        await using var stream = File.Create(path);
        await templateEngine.RenderAsync(
            Template,
            model,
            stream,
            cancellationToken);
        Info($"README: {path}");
        return 0;
    }

    private ReadmeCategory? CreateCategory(DiscoveredMatrixModule module)
    {
        var reportRoot = Path.Combine(
            buildPaths.SolutionDirectory,
            "reports",
            module.Metadata.ReportDirectory);
        var metadataRoot = Path.Combine(
            buildPaths.SolutionDirectory,
            "metadata",
            module.Metadata.ReportDirectory);
        var reportPath = Path.Combine(reportRoot, "benchmarks.json");
        var chartsPath = Path.Combine(metadataRoot, "charts.json");
        var librariesPath = Path.Combine(metadataRoot, "libraries.json");
        if (!File.Exists(reportPath) || !File.Exists(chartsPath) || !File.Exists(librariesPath))
        {
            Console.Error.WriteLine(
                $"WARNING: README data for {module.Metadata.Name} is incomplete.");
            return null;
        }

        var report = Read<BenchmarkReport>(reportPath);
        var charts = Read<MatrixChartCatalog>(chartsPath);
        var metadata = Read<MatrixLibraryMetadataCatalog>(librariesPath);
        var moduleLibraries = module.Metadata.Libraries.ToDictionary(
            library => library.Id,
            StringComparer.OrdinalIgnoreCase);
        var libraries = metadata.Libraries
            .Select(library =>
            {
                moduleLibraries.TryGetValue(library.Id, out var moduleLibrary);
                return new ReadmeLibrary(
                    library.Id,
                    moduleLibrary?.Name ?? library.Id,
                    moduleLibrary?.Version ?? string.Empty,
                    library.Description,
                    library.DocumentationUrl ?? library.RepositoryUrl,
                    RelativePath(
                        Path.Combine(metadataRoot, library.Logo)));
            })
            .OrderBy(library => library.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var overviews = charts.Groups
            .Select(group => new ReadmeChart(
                group.Name,
                RelativePath(Path.Combine(
                    reportRoot,
                    MatrixChartPaths.DirectoryName,
                    MatrixChartPaths.Overview(group)))))
            .ToArray();
        var features = report.Features
            .OrderBy(feature => feature.Order)
            .Select(feature => new ReadmeFeature(
                feature.Order,
                feature.Name,
                module.Metadata.FeatureMetadata.Features
                    .FirstOrDefault(item =>
                        item.Id.Equals(feature.Id, StringComparison.OrdinalIgnoreCase))
                    ?.Description,
                RelativePath(Path.Combine(
                    reportRoot,
                    MatrixChartPaths.DirectoryName,
                    MatrixChartPaths.Feature(feature)))))
            .ToArray();
        var rated = module.Metadata.LibraryMetadata.Libraries
            .Where(library => library.Rated)
            .Select(library => library.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var rating = MatrixRatings
            .Create(report, charts, rated.Contains)
            .Select((medals, index) => new ReadmeRating(
                index + 1,
                medals.LibraryId,
                medals.Name,
                MatrixScores.Format(medals.Points),
                MatrixScores.Format(medals.TimePoints),
                MatrixScores.Format(medals.MemoryPoints),
                medals.Maximum,
                medals.MetricMaximum,
                medals.Covered,
                medals.Scenarios,
                string.Join(
                    ", ",
                    medals.Awards.Select(award =>
                        $"{Place(award.Place)} in {award.GroupName}")),
                Breakdown(report, medals.LibraryId, rated.Contains)))
            .ToArray();
        return new ReadmeCategory(
            module.Metadata.Id,
            Anchor(module.Metadata.Name),
            module.Metadata.Name,
            libraries,
            overviews,
            features,
            rating);
    }

    /// <summary>
    /// The arithmetic behind one library's rating, one row per scenario. It comes
    /// from <see cref="MatrixScores.Explain"/>, the same per-cell function the
    /// rating sums, so the breakdown printed here cannot disagree with the total
    /// printed above it.
    /// </summary>
    private static ReadmeScore[] Breakdown(
        BenchmarkReport report,
        string libraryId,
        Func<string, bool> rated) =>
        MatrixScores
            .Explain(report.Features, libraryId, rated)
            .Select(detail => new ReadmeScore(
                detail.Name,
                Measurement(detail.Time, false),
                Measurement(detail.Time.Best, false),
                Points(detail.Time),
                Measurement(detail.Memory, true),
                Measurement(detail.Memory.Best, true),
                Points(detail.Memory)))
            .ToArray();

    /// <summary>An em dash where there is nothing to print keeps the columns aligned.</summary>
    private static string Measurement(MatrixScoreCell cell, bool memory) =>
        Measurement(cell.Value, memory);

    private static string Measurement(double? value, bool memory) =>
        value is null ? "—" : MatrixMetrics.Format(value.Value, memory);

    /// <summary>
    /// A metric nobody measured was never scored, and saying `0` there would
    /// invite the reader to look for points the library never had a chance at.
    /// </summary>
    private static string Points(MatrixScoreCell cell) =>
        cell.Contested ? MatrixScores.FormatExact(cell.Points) : "—";

    private static string Anchor(string value) =>
        AnchorRegex().Replace(value.ToLowerInvariant(), "-").Trim('-');

    private static string Place(int place) => place switch
    {
        1 => "gold",
        2 => "silver",
        _ => "bronze"
    };

    private string RelativePath(string path) =>
        Path.GetRelativePath(buildPaths.SolutionDirectory, path).Replace('\\', '/');

    private static T Read<T>(string path) =>
        JsonSerializer.Deserialize<T>(File.ReadAllText(path), JsonOptions)
        ?? throw new InvalidOperationException($"Cannot read '{path}'.");
    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex AnchorRegex();
}
