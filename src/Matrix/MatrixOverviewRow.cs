namespace Matrix;

/// <summary>
/// One library in a benchmark overview: its per-feature values in the order of
/// <see cref="MatrixOverview.Features"/>, plus the features it has no result for.
/// </summary>
public sealed record MatrixOverviewRow(
    string LibraryId,
    string Name,
    IReadOnlyList<double?> PerformanceValues,
    IReadOnlyList<double?> MemoryValues,
    IReadOnlyList<string> MissingFeatures);