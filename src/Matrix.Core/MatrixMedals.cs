namespace Matrix;

/// <summary>
/// One library's standing in a category: the points it scored across every
/// scenario, and what it won in the individual overview groups.
/// </summary>
/// <param name="TimePoints">
/// How close the library came to the fastest result in every scenario, summed.
/// See workflows/rating.md for the rule.
/// </param>
/// <param name="MemoryPoints">The same for allocated bytes.</param>
/// <param name="Covered">Scenarios the library completed.</param>
/// <param name="Scenarios">Scenarios the category measures.</param>
public sealed record MatrixMedals(
    string LibraryId,
    string Name,
    IReadOnlyList<MatrixMedal> Awards,
    double TimePoints,
    double MemoryPoints,
    int Covered,
    int Scenarios)
{
    /// <summary>Speed and economy count the same; the rating is their sum.</summary>
    public double Points => TimePoints + MemoryPoints;

    /// <summary>What a library that wins every scenario on both metrics scores.</summary>
    public int Maximum => MetricMaximum * MatrixRatings.Metrics;

    /// <summary>The most one metric can contribute.</summary>
    public int MetricMaximum => Scenarios * MatrixRatings.MaximumPoints;
}
