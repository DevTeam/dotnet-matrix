// ReSharper disable CheckNamespace
namespace Matrix.Web;

/// <inheritdoc cref="IMatrixView"/>
internal sealed class MatrixView : IMatrixView
{
    public bool IsSelected(
        CategoryReport report,
        IReadOnlySet<string> selectedLibraries,
        string libraryId) =>
        selectedLibraries.Contains(Key(report.Category.Id, libraryId));

    public IEnumerable<MatrixLibrary> Libraries(CategoryReport report) =>
        (report.Features?.Libraries ?? [])
        .Concat((report.Benchmarks?.Libraries ?? [])
            .Select(library => new MatrixLibrary(
                library.Id,
                library.Name,
                library.Package,
                library.Version,
                library.Baseline)))
        .DistinctBy(library => library.Id, StringComparer.OrdinalIgnoreCase)
        .OrderBy(library => library.Name, StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<(int Order, string Id, string Name)> Features(CategoryReport report) =>
        (report.Features?.Features ?? [])
        .Select(feature => (feature.Order, feature.Id, feature.Name))
        .Concat((report.Benchmarks?.Features ?? [])
            .Select(feature => (feature.Order, feature.Id, feature.Name)))
        .DistinctBy(feature => feature.Id, StringComparer.OrdinalIgnoreCase)
        .OrderBy(feature => feature.Order)
        .ToArray();

    public CategoryReport Restrict(
        CategoryReport report,
        IReadOnlySet<string> selectedFeatures)
    {
        // Nothing is left out, so nothing is rebuilt: the default view compares the
        // whole category and pays nothing for the ability to narrow it.
        if (Features(report).All(feature => Chosen(feature.Id)))
        {
            return report;
        }

        return report with
        {
            Features = report.Features is { } features
                ? features with
                {
                    Features = [.. features.Features.Where(feature => Chosen(feature.Id))]
                }
                : null,
            Benchmarks = report.Benchmarks is { } benchmarks
                ? benchmarks with
                {
                    Features = [.. benchmarks.Features.Where(feature => Chosen(feature.Id))]
                }
                : null,
            ChartCatalog = report.ChartCatalog is { } charts
                ? charts with
                {
                    Groups =
                    [
                        .. charts.Groups
                            .Select(group => group with
                            {
                                Features = [.. group.Features.Where(Chosen)]
                            })
                            .Where(group => group.Features.Count > 0)
                    ]
                }
                : null
        };

        bool Chosen(string featureId) =>
            selectedFeatures.Contains(Key(report.Category.Id, featureId));
    }

    public MatrixLibraryMetadata? Metadata(CategoryReport report, string libraryId) =>
        report.LibraryCatalog?.Libraries.FirstOrDefault(metadata =>
            metadata.Id.Equals(libraryId, StringComparison.OrdinalIgnoreCase));

    public bool IsRated(CategoryReport report, string libraryId) =>
        Metadata(report, libraryId) is { Rated: true };

    private static MatrixFeatureMetadata? FeatureMetadata(CategoryReport report, string featureId) =>
        report.FeatureCatalog?.Features.FirstOrDefault(feature =>
            feature.Id.Equals(featureId, StringComparison.OrdinalIgnoreCase));

    public bool IsFeatureRated(CategoryReport report, string featureId) =>
        FeatureMetadata(report, featureId) is not { Rated: false };

    public IReadOnlyList<BenchmarkReportEntry> RatedFeatures(CategoryReport report) =>
        report.Benchmarks?.Features
            .Where(feature => IsFeatureRated(report, feature.Id))
            .ToArray()
        ?? [];

    public (int Supported, int Rated) FeatureCoverage(CategoryReport report, string featureId) =>
        MatrixCoverage.Feature(report.Features, report.LibraryCatalog, featureId);

    public string? FeatureNotRatedReason(CategoryReport report, string featureId)
    {
        if (FeatureMetadata(report, featureId) is not { Rated: false, Reason: { Length: > 0 } reason })
        {
            return null;
        }

        var (supported, rated) = FeatureCoverage(report, featureId);
        return rated > 0
            ? $"{reason} ({supported} of {rated} rated libraries support this.)"
            : reason;
    }

    public string? Logo(CategoryReport report, string libraryId) =>
        Metadata(report, libraryId)?.Logo is { Length: > 0 } logo ? logo : null;

    public string? FeatureDescription(CategoryReport report, string featureId) =>
        FeatureMetadata(report, featureId)
            ?.Description is { Length: > 0 } description
            ? description
            : null;

    public MatrixOverview? Overview(
        CategoryReport report,
        MatrixChartGroup group,
        IReadOnlySet<string> selectedLibraries) =>
        report.Benchmarks is not { } benchmarks
            ? null
            : MatrixOverviews.Create(
                benchmarks,
                group,
                libraryId => IsSelected(report, selectedLibraries, libraryId),
                libraryId => IsRated(report, libraryId));

    public MatrixOverview? RatedOverview(
        CategoryReport report,
        MatrixChartGroup group,
        IReadOnlySet<string> selectedLibraries) =>
        report.Benchmarks is not { } benchmarks
            ? null
            : MatrixOverviews.Create(
                benchmarks,
                group,
                libraryId => IsRated(report, libraryId)
                             && IsSelected(report, selectedLibraries, libraryId));

    public IReadOnlyList<MatrixScenarioRow> Scenarios(
        CategoryReport report,
        BenchmarkReportEntry feature,
        IReadOnlySet<string> selectedLibraries) =>
        report.Benchmarks is not { } benchmarks
            ? []
            : MatrixScenarios.Create(
                benchmarks,
                feature,
                libraryId => IsSelected(report, selectedLibraries, libraryId));

    public IReadOnlyList<MatrixScenarioRow> Order(
        IEnumerable<MatrixScenarioRow> rows,
        bool memory) =>
        MatrixScenarios.Order(rows, memory);

    public double? Value(MatrixScenarioRow row, bool memory) =>
        MatrixScenarios.Value(row, memory);

    public double Worst(IEnumerable<MatrixScenarioRow> rows, bool memory) =>
        MatrixScenarios.Maximum(rows, memory);

    public double Best(IEnumerable<MatrixScenarioRow> rows, bool memory) =>
        MatrixScenarios.Minimum(rows, memory);

    /// <summary>
    /// A unit separator joins the two halves. It cannot occur in an id, so no
    /// pair of category and library — or category and scenario — can collide with
    /// another, and the escape is spelled out rather than typed as an invisible
    /// character.
    /// </summary>
    private const char Separator = '';

    private static string Key(string categoryId, string itemId) =>
        $"{categoryId}{Separator}{itemId}";
}
