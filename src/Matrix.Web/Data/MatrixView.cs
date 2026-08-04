// ReSharper disable CheckNamespace
namespace Matrix.Web;

/// <inheritdoc cref="IMatrixView"/>
internal sealed class MatrixView : IMatrixView
{
    public bool IsSelected(
        CategoryReport report,
        IReadOnlySet<string> selectedLibraries,
        string libraryId) =>
        selectedLibraries.Contains(LibraryKey(report.Category.Id, libraryId));

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

    public MatrixLibraryMetadata? Metadata(CategoryReport report, string libraryId) =>
        report.LibraryCatalog?.Libraries.FirstOrDefault(metadata =>
            metadata.Id.Equals(libraryId, StringComparison.OrdinalIgnoreCase));

    public bool IsRated(CategoryReport report, string libraryId) =>
        Metadata(report, libraryId) is { Rated: true };

    public string? Logo(CategoryReport report, string libraryId) =>
        Metadata(report, libraryId)?.Logo is { Length: > 0 } logo ? logo : null;

    public string? FeatureDescription(CategoryReport report, string featureId) =>
        report.FeatureCatalog?.Features
            .FirstOrDefault(feature =>
                feature.Id.Equals(featureId, StringComparison.OrdinalIgnoreCase))
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
    /// pair of category and library can collide with another, and the escape is
    /// spelled out rather than typed as an invisible character.
    /// </summary>
    private const char Separator = '';

    private static string LibraryKey(string categoryId, string libraryId) =>
        $"{categoryId}{Separator}{libraryId}";
}
