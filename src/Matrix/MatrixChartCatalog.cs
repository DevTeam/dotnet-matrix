namespace Matrix;

public sealed record MatrixChartCatalog(
    int SchemaVersion,
    IReadOnlyList<MatrixChartGroup> Groups);