namespace Build.Targets;

public sealed record ReadmeCategory(
    string Id,
    string Anchor,
    string Name,
    IReadOnlyList<ReadmeLibrary> Libraries,
    IReadOnlyList<ReadmeChart> Overviews,
    IReadOnlyList<ReadmeFeature> Features,
    IReadOnlyList<ReadmeRating> Rating,
    /// <summary>
    /// Solution-relative path to this category's full report, e.g.
    /// <c>reports/CsvProcessing/README.md</c> — where README.md links out to
    /// the detail this category's section no longer carries inline.
    /// </summary>
    string ReportPath,
    /// <summary>
    /// The category-wide standings, rendered as an image (see
    /// <c>ReportChartsTarget.RenderRating</c>) — README.md shows this instead of
    /// setting <see cref="Rating"/> as a Markdown table. Null when the category
    /// is not currently rated.
    /// </summary>
    ReadmeChart? RatingChart);
