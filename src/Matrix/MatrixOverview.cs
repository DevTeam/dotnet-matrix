// ReSharper disable NotAccessedPositionalProperty.Global
// ReSharper disable UseCollectionExpression
namespace Matrix;

/// <summary>
/// The data behind one overview chart. Shared by the build, which renders it to a
/// PNG for the readme, and by the web application, which renders it as markup.
/// Rows are ordered by the points scored inside the group, the same rule and the
/// same order the category standings use. The bars keep showing measured totals,
/// so a shorter bar can sit below a longer one.
/// </summary>
public sealed record MatrixOverview(
    MatrixChartGroup Group,
    IReadOnlyList<BenchmarkReportEntry> Features,
    IReadOnlyList<MatrixOverviewRow> Rows,
    double MaximumTime,
    double MaximumMemory)
{
    /// <summary>The most a library can score in this group.</summary>
    public int Maximum => MatrixScores.Maximum(Features.Count);
}
