namespace Matrix;

/// <inheritdoc cref="IMatrixReportInvariants"/>
/// <remarks>
/// <see cref="MatrixScores"/> looks a library's result up by id alone and sums
/// one entry per match, <see cref="MatrixOverviews"/> picks "the" result the
/// same way, and <see cref="MatrixBenchmarkRunner"/>'s merge replaces by id. A
/// second entry for the same library — e.g. from a benchmark configuration that
/// resolves to more than one BenchmarkDotNet job — would be silently
/// double-counted, silently picked, or silently dropped rather than reported.
/// Callers producing or reading a report call this so that breach turns into a
/// loud failure instead.
/// </remarks>
public sealed class MatrixReportInvariants : IMatrixReportInvariants
{
    public void EnsureUniqueResultPerLibrary(IEnumerable<BenchmarkReportEntry> features)
    {
        var duplicates = features
            .SelectMany(feature => feature.Results
                .GroupBy(result => result.LibraryId, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Count() > 1)
                .Select(group => $"{feature.Id}/{group.Key} ({group.Count()} results)"))
            .ToArray();
        if (duplicates.Length == 0)
        {
            return;
        }

        throw new InvalidOperationException(
            "Benchmark report has more than one result for the same library within a "
            + "feature, which scoring and overview code represents as at most one: "
            + string.Join(", ", duplicates) + ".");
    }
}
