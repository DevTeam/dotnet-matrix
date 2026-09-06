namespace Matrix;

/// <summary>
/// The category rating: the scoring rule applied to every scenario of the
/// report. See <see cref="MatrixRatings"/> and workflows/rating.md.
/// </summary>
public interface IMatrixRatings
{
    IReadOnlyList<MatrixMedals> Create(
        BenchmarkReport report,
        MatrixChartCatalog charts,
        Func<string, bool> isRated,
        Func<string, bool> isFeatureRated,
        Func<string, bool>? includeLibrary = null);
}
