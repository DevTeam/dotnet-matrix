namespace Matrix;

/// <inheritdoc cref="IMatrixOverviews"/>
public sealed class MatrixOverviews(IMatrixScores scores) : IMatrixOverviews
{
    public MatrixOverview? Create(
        BenchmarkReport report,
        MatrixChartGroup group,
        Func<string, bool>? includeLibrary = null,
        Func<string, bool>? isRated = null)
    {
        var features = group.Features
            .Select(id => report.Features.FirstOrDefault(feature =>
                feature.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
            .Where(feature => feature is not null)
            .Select(feature => feature!)
            .ToArray();
        if (features.Length == 0)
        {
            return null;
        }

        var libraries = report.Libraries
            .Where(library => includeLibrary?.Invoke(library.Id) ?? true)
            .ToArray();
        var competitors = libraries.Where(library => Rated(library.Id)).ToArray();
        var score = scores.Create(
            features,
            competitors.Select(library => library.Id),
            CompetesForBest);

        // By score, rated and reference rows alike: a row sits where its own
        // number puts it. A reference row's score just never assigns it a place
        // in the rating — see MatrixOverviewRow.Rated and the callers of Rank().
        var rows = libraries
            .Select(library => CreateRow(library, features, score, Rated(library.Id), CompetesForBest))
            .OrderByDescending(row => row.Points)
            .ThenBy(row => row.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        return new MatrixOverview(
            group,
            features,
            rows,
            rows.Select(row => MatrixMetrics.Total(row.PerformanceValues)).DefaultIfEmpty().Max(),
            rows.Select(row => MatrixMetrics.Total(row.MemoryValues)).DefaultIfEmpty().Max(),
            scores.Maximum(features.Length));

        bool Rated(string libraryId) => isRated?.Invoke(libraryId) ?? true;

        bool CompetesForBest(string libraryId) =>
            (includeLibrary?.Invoke(libraryId) ?? true) && Rated(libraryId);
    }

    private MatrixOverviewRow CreateRow(
        BenchmarkLibrary library,
        IReadOnlyList<BenchmarkReportEntry> features,
        IReadOnlyDictionary<string, MatrixScore> score,
        bool rated,
        Func<string, bool> competesForBest)
    {
        var results = features
            .Select(feature => feature.Results.FirstOrDefault(result =>
                result.Successful
                && result.LibraryId.Equals(library.Id, StringComparison.OrdinalIgnoreCase)))
            .ToArray();
        var earned = rated
            ? score.GetValueOrDefault(library.Id) ?? new MatrixScore(0, 0, 0)
            : Reference(features, library.Id, competesForBest);
        return new MatrixOverviewRow(
            library.Id,
            library.Name,
            [.. results.Select(result => result?.MeanNanoseconds)],
            [.. results.Select(result => result?.AllocatedBytesPerOperation)],
            [.. features.Where((_, index) => results[index] is null).Select(feature => feature.Name)],
            earned.Time,
            earned.Memory,
            rated);
    }

    /// <summary>
    /// What a library outside the rating would have scored against the field it
    /// is compared with. Runs through <see cref="IMatrixScores.Explain"/> rather
    /// than the bulk <see cref="IMatrixScores.Create"/> above, because such a
    /// library must never enter that bulk pass — doing so would let it help
    /// define <c>best</c>, which is exactly what keeping it out of the rating
    /// forbids. See workflows/rating.md.
    /// </summary>
    private MatrixScore Reference(
        IReadOnlyList<BenchmarkReportEntry> features,
        string libraryId,
        Func<string, bool> competesForBest)
    {
        var details = scores.Explain(features, libraryId, competesForBest);
        return new MatrixScore(
            details.Sum(detail => detail.Time.Points),
            details.Sum(detail => detail.Memory.Points),
            details.Count(detail => detail.Covered));
    }
}
