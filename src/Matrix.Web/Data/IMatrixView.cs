// ReSharper disable CheckNamespace
namespace Matrix.Web;

/// <summary>
/// What a component may ask a report about the libraries and scenarios in it.
/// Everything here is a lookup or a projection over one <see cref="CategoryReport"/>;
/// nothing here scores, formats or colours anything.
/// </summary>
internal interface IMatrixView
{
    /// <summary>The library is in the comparison the visitor has chosen.</summary>
    bool IsSelected(
        CategoryReport report,
        IReadOnlySet<string> selectedLibraries,
        string libraryId);

    /// <summary>
    /// Every library the report knows, from either half of it, by name.
    /// </summary>
    IEnumerable<MatrixLibrary> Libraries(CategoryReport report);

    /// <summary>
    /// Every scenario the report knows, from either half of it, in the order the
    /// category declares. The feature report wins the name: it is the one the
    /// matrix shows.
    /// </summary>
    IReadOnlyList<(int Order, string Id, string Name)> Features(CategoryReport report);

    /// <summary>
    /// The same report with everything outside the chosen scenarios removed: the
    /// matrix rows, the benchmark entries and the scenarios of every overview
    /// group. A group left without scenarios is dropped, because it no longer
    /// describes anything. Every number the interface shows is derived from these
    /// lists, so restricting them here is what makes the rating agree with the
    /// matrix instead of each view deciding for itself what to leave out.
    /// </summary>
    CategoryReport Restrict(CategoryReport report, IReadOnlySet<string> selectedFeatures);

    MatrixLibraryMetadata? Metadata(CategoryReport report, string libraryId);

    /// <summary>
    /// Only declared libraries compete, and only while they keep the flag.
    /// Baseline and rating are independent metadata decisions.
    /// </summary>
    bool IsRated(CategoryReport report, string libraryId);

    string? Logo(CategoryReport report, string libraryId);

    string? FeatureDescription(CategoryReport report, string featureId);

    /// <summary>
    /// One group of the report, ordered by its score. Libraries outside the rating
    /// are still drawn, as a reference that sets no best result.
    /// </summary>
    MatrixOverview? Overview(
        CategoryReport report,
        MatrixChartGroup group,
        IReadOnlySet<string> selectedLibraries);

    /// <summary>
    /// The same group with the unrated libraries left out entirely, which is what
    /// decides who holds a place in it.
    /// </summary>
    MatrixOverview? RatedOverview(
        CategoryReport report,
        MatrixChartGroup group,
        IReadOnlySet<string> selectedLibraries);

    /// <summary>The measured rows of one scenario, for the compared libraries.</summary>
    IReadOnlyList<MatrixScenarioRow> Scenarios(
        CategoryReport report,
        BenchmarkReportEntry feature,
        IReadOnlySet<string> selectedLibraries);

    /// <summary>Best first; rows without a value for the metric sort last.</summary>
    IReadOnlyList<MatrixScenarioRow> Order(IEnumerable<MatrixScenarioRow> rows, bool memory);

    double? Value(MatrixScenarioRow row, bool memory);

    /// <summary>The worst result among the rows, which is what a bar is drawn against.</summary>
    double Worst(IEnumerable<MatrixScenarioRow> rows, bool memory);

    /// <summary>The best result among the rows, which is what a ratio is read against.</summary>
    double Best(IEnumerable<MatrixScenarioRow> rows, bool memory);
}
