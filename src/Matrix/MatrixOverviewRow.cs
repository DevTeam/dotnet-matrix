namespace Matrix;

/// <summary>
/// One library in a benchmark overview: its per-feature values in the order of
/// <see cref="MatrixOverview.Features"/>, the features it has no result for, and
/// what it scored inside the group. The score comes from <see cref="MatrixScores"/>,
/// the rule the category rating uses, so a group standing and the overall
/// standing are read the same way.
/// </summary>
/// <param name="Rated">
/// False for a library the category keeps out of its rating, such as a
/// hand-written baseline. It is drawn as a reference but scores nothing and does
/// not set the best result the others are measured against.
/// </param>
public sealed record MatrixOverviewRow(
    string LibraryId,
    string Name,
    IReadOnlyList<double?> PerformanceValues,
    IReadOnlyList<double?> MemoryValues,
    IReadOnlyList<string> MissingFeatures,
    double TimePoints,
    double MemoryPoints,
    bool Rated)
{
    public double Points => TimePoints + MemoryPoints;
}
