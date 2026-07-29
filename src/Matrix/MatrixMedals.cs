namespace Matrix;

/// <summary>
/// What one library won across the overview groups of a category.
/// </summary>
public sealed record MatrixMedals(
    string LibraryId,
    string Name,
    IReadOnlyList<MatrixMedal> Awards)
{
    public int Gold => Count(1);

    public int Silver => Count(2);

    public int Bronze => Count(3);

    public int Total => Awards.Count;

    public int Count(int place) => Awards.Count(award => award.Place == place);
}