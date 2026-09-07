using Matrix;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
// ReSharper disable UseCollectionExpression

namespace Build.Targets;

internal sealed partial class ReadmeTarget(
    IBuildPaths buildPaths,
    IMetadataTarget metadataTarget,
    IReportChartsTarget reportChartsTarget,
    ITemplateEngine templateEngine,
    IMatrixReportReader reportReader,
    IMatrixScores scores,
    IMatrixRatings ratings) : IReadmeTarget
{
    private const string Template = "/Templates/Readme.cshtml";
    private const string CategoryReportTemplate = "/Templates/CategoryReport.cshtml";
    private const string ReproduceTemplate = "/Templates/Reproduce.cshtml";
    private const string DeepLinkingTemplate = "/Templates/DeepLinking.cshtml";
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
        await RenderToFileAsync(
            Template,
            model,
            Path.Combine(buildPaths.SolutionDirectory, "README.md"),
            cancellationToken);
        await RenderToFileAsync(
            ReproduceTemplate,
            model,
            Path.Combine(buildPaths.SolutionDirectory, "docs", "reproduce.md"),
            cancellationToken);
        await RenderToFileAsync(
            DeepLinkingTemplate,
            model,
            Path.Combine(buildPaths.SolutionDirectory, "docs", "deep-linking.md"),
            cancellationToken);
        foreach (var category in categories)
        {
            var reportModel = new CategoryReportModel(applicationUrl, RerootToReport(category));
            await RenderToFileAsync(
                CategoryReportTemplate,
                reportModel,
                Path.Combine(buildPaths.SolutionDirectory, category.ReportPath),
                cancellationToken);
        }

        return 0;
    }

    /// <summary>
    /// Renders a template and writes it to <paramref name="path"/>, creating the
    /// containing directory when a new documentation area (such as `docs/`) does
    /// not exist yet.
    /// </summary>
    private async Task RenderToFileAsync<TModel>(
        string template,
        TModel model,
        string path,
        CancellationToken cancellationToken)
    {
        using var buffer = new MemoryStream();
        await templateEngine.RenderAsync(template, model, buffer, cancellationToken);
        // The encoder still escapes ' and " — the two characters HtmlEncoder
        // treats as always unsafe regardless of its allowed-character settings,
        // even though Markdown has no attribute context for them to break out
        // of. < > & stay escaped: generated pages embed real HTML (<details>)
        // that an unescaped one would corrupt.
        var text = Encoding.UTF8.GetString(buffer.ToArray())
            .Replace("&#x27;", "'")
            .Replace("&quot;", "\"");
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(path, text, new UTF8Encoding(false), cancellationToken);
        Info($"Generated: {path}");
    }

    /// <summary>
    /// The top-level README links every image and logo relative to the solution
    /// root. A category's own report lives inside its report directory instead,
    /// so every one of those paths is rewritten relative to that directory —
    /// otherwise `reports/CsvProcessing/README.md` would look for its own charts
    /// under `reports/CsvProcessing/reports/CsvProcessing/charts/...`.
    /// </summary>
    private ReadmeCategory RerootToReport(ReadmeCategory category)
    {
        var reportRoot = Path.Combine(buildPaths.SolutionDirectory, category.ReportPath, "..");
        string Reroot(string solutionRelativePath) =>
            Path.GetRelativePath(
                    reportRoot,
                    Path.Combine(buildPaths.SolutionDirectory, solutionRelativePath))
                .Replace('\\', '/');
        return category with
        {
            Libraries = category.Libraries
                .Select(library => library with { Logo = Reroot(library.Logo) })
                .ToArray(),
            Overviews = category.Overviews
                .Select(overview => overview with { Path = Reroot(overview.Path) })
                .ToArray(),
            Features = category.Features
                .Select(feature => feature with { ChartPath = Reroot(feature.ChartPath) })
                .ToArray(),
            RatingChart = category.RatingChart is null
                ? null
                : category.RatingChart with { Path = Reroot(category.RatingChart.Path) }
        };
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
        var featuresPath = Path.Combine(reportRoot, "features.json");
        var chartsPath = Path.Combine(metadataRoot, "charts.json");
        var librariesPath = Path.Combine(metadataRoot, "libraries.json");
        if (!reportReader.Exists(reportPath) || !reportReader.Exists(chartsPath) || !reportReader.Exists(librariesPath))
        {
            Console.Error.WriteLine(
                $"WARNING: README data for {module.Metadata.Name} is incomplete.");
            return null;
        }

        var report = Read<BenchmarkReport>(reportPath);
        // Coverage — "supported by N of M rated libraries" — needs the validation
        // report, which benchmarks.json does not carry. Missing is not fatal: the
        // count is only ever appended to a Rated: false reason, and a category
        // with no not-rated scenario never asks for it.
        var featureReport = reportReader.Exists(featuresPath) ? Read<FeatureReport>(featuresPath) : null;
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
            .Select(feature =>
            {
                var featureMetadata = module.Metadata.FeatureMetadata.Features
                    .FirstOrDefault(item =>
                        item.Id.Equals(feature.Id, StringComparison.OrdinalIgnoreCase));
                return new ReadmeFeature(
                    feature.Id,
                    feature.Order,
                    feature.Name,
                    featureMetadata?.Description,
                    RelativePath(Path.Combine(
                        reportRoot,
                        MatrixChartPaths.DirectoryName,
                        MatrixChartPaths.Feature(feature))),
                    NotRatedReason(featureMetadata, featureReport, metadata, feature.Id));
            })
            .ToArray();
        var rated = module.Metadata.LibraryMetadata.Libraries
            .Where(library => library.Rated)
            .Select(library => library.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var ratedFeatures = module.Metadata.FeatureMetadata.Features
            .Where(feature => feature.Rated)
            .Select(feature => feature.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var rating = ratings
            .Create(report, charts, rated.Contains, ratedFeatures.Contains)
            .Select((medals, index) => new ReadmeRating(
                index + 1,
                medals.LibraryId,
                medals.Name,
                scores.Format(medals.Points),
                scores.Format(medals.TimePoints),
                scores.Format(medals.MemoryPoints),
                medals.Maximum,
                medals.MetricMaximum,
                medals.Covered,
                medals.Scenarios,
                string.Join(
                    ", ",
                    medals.Awards.Select(award =>
                        $"{Place(award.Place)} in {award.GroupName}")),
                Breakdown(report, medals.LibraryId, rated.Contains, ratedFeatures.Contains)))
            .ToArray();
        var ratingChart = rating.Length == 0
            ? null
            : new ReadmeChart(
                module.Metadata.Name,
                RelativePath(Path.Combine(
                    reportRoot,
                    MatrixChartPaths.DirectoryName,
                    MatrixChartPaths.Rating())));
        return new ReadmeCategory(
            module.Metadata.Id,
            Anchor(module.Metadata.Name),
            module.Metadata.Name,
            libraries,
            overviews,
            features,
            rating,
            RelativePath(Path.Combine(reportRoot, "README.md")),
            ratingChart);
    }

    /// <summary>
    /// The authored reason a scenario is not rated, with the current "N of M"
    /// support count appended via <see cref="MatrixCoverage"/> — the same call
    /// the Web application makes, so the two surfaces can never quote different
    /// numbers for the same scenario.
    /// </summary>
    private static string? NotRatedReason(
        MatrixFeatureMetadata? featureMetadata,
        FeatureReport? featureReport,
        MatrixLibraryMetadataCatalog libraries,
        string featureId)
    {
        if (featureMetadata is not { Rated: false, Reason: { Length: > 0 } reason })
        {
            return null;
        }

        var (supported, rated) = MatrixCoverage.Feature(featureReport, libraries, featureId);
        return rated > 0
            ? $"{reason} ({supported} of {rated} rated libraries support this.)"
            : reason;
    }

    /// <summary>
    /// The arithmetic behind one library's rating, one row per scenario. It comes
    /// from <see cref="MatrixScores.Explain"/>, the same per-cell function the
    /// rating sums, so the breakdown printed here cannot disagree with the total
    /// printed above it.
    /// </summary>
    private ReadmeScore[] Breakdown(
        BenchmarkReport report,
        string libraryId,
        Func<string, bool> rated,
        Func<string, bool> featureRated) =>
        scores
            .Explain(
                report.Features.Where(feature => featureRated(feature.Id)),
                libraryId,
                rated)
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
    private string Points(MatrixScoreCell cell) =>
        cell.Contested ? scores.FormatExact(cell.Points) : "—";

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

    private T Read<T>(string path) =>
        reportReader.Read<T>(path, JsonOptions)
        ?? throw new InvalidOperationException($"Cannot read '{path}'.");
    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex AnchorRegex();
}
