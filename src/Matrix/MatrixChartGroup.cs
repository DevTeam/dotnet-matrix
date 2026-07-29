namespace Matrix;

public sealed record MatrixChartGroup(
    string Id,
    string Name,
    IReadOnlyList<string> Features);