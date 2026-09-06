// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed record MatrixChartCatalog(
    int SchemaVersion,
    IReadOnlyList<MatrixChartGroup> Groups);