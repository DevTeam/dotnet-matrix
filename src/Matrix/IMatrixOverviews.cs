namespace Matrix;

/// <summary>
/// One overview chart group, built from a report. See <see cref="MatrixOverviews"/>.
/// </summary>
public interface IMatrixOverviews
{
    /// <summary>
    /// Returns null when the group names no feature present in the report.
    /// Rows come back ordered by the points they scored inside the group, using
    /// <see cref="IMatrixScores"/> — the rule the category standings use — so the
    /// two ratings are read and computed the same way.
    /// </summary>
    /// <param name="report"></param>
    /// <param name="group"></param>
    /// <param name="includeLibrary">
    /// Optional library filter; the web application passes the current selection.
    /// </param>
    /// <param name="isRated">
    /// Which libraries take part in the rating. A library outside it is still
    /// drawn — a hand-written baseline is the most useful row on the chart — and
    /// still earns a real score against the field's best, so its row is exactly
    /// as informative as any other. It only never defines that best itself and
    /// never takes a place: see <see cref="MatrixOverviewRow.Rated"/>.
    /// </param>
    MatrixOverview? Create(
        BenchmarkReport report,
        MatrixChartGroup group,
        Func<string, bool>? includeLibrary = null,
        Func<string, bool>? isRated = null);
}
