namespace Build.Targets;

/// <summary>
/// One category's section of the root README.
/// </summary>
/// <param name="ReportPath">
/// Solution-relative path to this category's full report, e.g.
/// <c>reports/CsvProcessing/README.md</c> — where README.md links out to
/// the detail this category's section no longer carries inline.
/// </param>
/// <param name="RatingChart">
/// The category-wide standings, rendered as an image (see
/// <c>ReportChartsTarget.RenderRating</c>) — README.md shows this instead of
/// setting <see cref="Rating"/> as a Markdown table. Null when the category
/// is not currently rated.
/// </param>
public sealed record ReadmeCategory(
    string Id,
    string Anchor,
    string Name,
    IReadOnlyList<ReadmeLibrary> Libraries,
    IReadOnlyList<ReadmeChart> Overviews,
    IReadOnlyList<ReadmeFeature> Features,
    IReadOnlyList<ReadmeRating> Rating,
    string ReportPath,
    ReadmeChart? RatingChart);
