namespace Matrix;

/// <summary>
/// The one scoring rule of the project, applied to whatever set of scenarios it
/// is given. See <see cref="MatrixScores"/> and workflows/rating.md.
/// </summary>
public interface IMatrixScores
{
    /// <summary>The most a library can earn over <paramref name="scenarios"/>.</summary>
    int Maximum(int scenarios);

    /// <summary>
    /// A score is only ever read against the maximum, so whole points are enough
    /// above ten; below it the fraction is the whole story.
    /// </summary>
    string Format(double points);

    /// <summary>
    /// The same number where it has to add up: one decimal above ten, for a
    /// breakdown that has to sum to the total printed beside it.
    /// </summary>
    string FormatExact(double points);

    /// <summary>
    /// <see cref="Format"/>, or nothing once <paramref name="points"/> passes
    /// <paramref name="maximum"/>.
    /// </summary>
    string FormatWithinMax(double points, double maximum);

    IReadOnlyDictionary<string, MatrixScore> Create(
        IEnumerable<BenchmarkReportEntry> features,
        IEnumerable<string> libraryIds,
        Func<string, bool>? includeLibrary = null);

    /// <summary>
    /// The same scores <see cref="Create"/> sums, itemised for one library so the
    /// number in the interface can be checked against the measurements it came
    /// from.
    /// </summary>
    IReadOnlyList<MatrixScoreDetail> Explain(
        IEnumerable<BenchmarkReportEntry> features,
        string libraryId,
        Func<string, bool>? includeLibrary = null);
}
