namespace Matrix;

/// <summary>
/// A report is assumed to carry at most one <see cref="BenchmarkResult"/> per
/// library within a feature. See <see cref="MatrixReportInvariants"/>.
/// </summary>
public interface IMatrixReportInvariants
{
    /// <summary>
    /// Throws <see cref="InvalidOperationException"/> when any feature carries
    /// more than one result for the same library.
    /// </summary>
    void EnsureUniqueResultPerLibrary(IEnumerable<BenchmarkReportEntry> features);
}
