namespace Matrix;

/// <summary>
/// One library's standing in a category: the points it scored across every
/// scenario, and what it won in the individual overview groups.
/// </summary>
/// <param name="Points">
/// How close the library came to the best result in every scenario, summed.
/// See workflows/rating.md for the rule.
/// </param>
/// <param name="Covered">Scenarios the library completed.</param>
/// <param name="Scenarios">Scenarios the category measures.</param>
public sealed record MatrixMedals(
    string LibraryId,
    string Name,
    IReadOnlyList<MatrixMedal> Awards,
    int Points,
    int Covered,
    int Scenarios)
{
    /// <summary>What a library that wins every scenario scores.</summary>
    public int Maximum => Scenarios * MatrixRatings.MaximumPoints;

    public int Gold => Count(1);

    public int Silver => Count(2);

    public int Bronze => Count(3);

    public int Total => Awards.Count;

    public int Count(int place) => Awards.Count(award => award.Place == place);
}
