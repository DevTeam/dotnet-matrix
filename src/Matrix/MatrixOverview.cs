// ReSharper disable NotAccessedPositionalProperty.Global
// ReSharper disable UseCollectionExpression
namespace Matrix;

/// <summary>
/// The data behind one overview chart. Shared by the build, which renders it to a
/// PNG for the readme, and by the web application, which renders it as markup.
/// </summary>
public sealed record MatrixOverview(
    MatrixChartGroup Group,
    IReadOnlyList<BenchmarkReportEntry> Features,
    IReadOnlyList<MatrixOverviewRow> Ranked,
    IReadOnlyList<MatrixOverviewRow> Unranked,
    double MaximumTime,
    double MaximumMemory);