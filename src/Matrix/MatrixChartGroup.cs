namespace Matrix;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed record MatrixChartGroup(
    string Id,
    string Name,
    IReadOnlyList<string> Features);