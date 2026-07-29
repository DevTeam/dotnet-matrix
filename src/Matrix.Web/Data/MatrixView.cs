// ReSharper disable CheckNamespace
namespace Matrix.Web;

internal static class MatrixView
{
    public static bool IsSelected(
        CategoryReport report,
        IReadOnlySet<string> selectedLibraries,
        string libraryId) =>
        selectedLibraries.Contains(LibraryKey(report.Category.Id, libraryId));

    public static IEnumerable<MatrixLibrary> Libraries(CategoryReport report) =>
        (report.Features?.Libraries ?? [])
        .Concat((report.Benchmarks?.Libraries ?? [])
            .Select(library => new MatrixLibrary(
                library.Id,
                library.Name,
                library.Package ?? string.Empty,
                library.Version ?? string.Empty)))
        .DistinctBy(library => library.Id, StringComparer.OrdinalIgnoreCase)
        .OrderBy(library => library.Name, StringComparer.OrdinalIgnoreCase);

    public static MatrixLibraryMetadata? Metadata(CategoryReport report, string libraryId) =>
        report.LibraryCatalog?.Libraries.FirstOrDefault(metadata =>
            metadata.Id.Equals(libraryId, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Only declared libraries compete, and only while they keep the flag. The
    /// hand-coded baseline has no catalog entry, so it never enters the rating.
    /// </summary>
    public static bool IsRated(CategoryReport report, string libraryId) =>
        Metadata(report, libraryId) is { Rated: true };

    public static IReadOnlyList<MatrixMedals> Rating(
        CategoryReport report,
        IReadOnlySet<string> selectedLibraries) =>
        report.Benchmarks is null || report.ChartCatalog is null
            ? []
            : MatrixRatings.Create(
                report.Benchmarks,
                report.ChartCatalog,
                libraryId => IsRated(report, libraryId),
                libraryId => IsSelected(report, selectedLibraries, libraryId));

    public static string? FeatureDescription(CategoryReport report, string featureId) =>
        report.FeatureCatalog?.Features
            .FirstOrDefault(feature =>
                feature.Id.Equals(featureId, StringComparison.OrdinalIgnoreCase))
            ?.Description is { Length: > 0 } description
            ? description
            : null;

    public static string? Logo(CategoryReport report, string libraryId) =>
        Metadata(report, libraryId)?.Logo is { Length: > 0 } logo ? logo : null;

    private static string LibraryKey(string categoryId, string libraryId) =>
        $"{categoryId}\u001f{libraryId}";
}
