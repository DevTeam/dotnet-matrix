namespace Matrix;

public static class MatrixOverviews
{
    /// <summary>
    /// Returns null when the group names no feature present in the report.
    /// Rows come back ordered by the points they scored inside the group, using
    /// <see cref="MatrixScores"/> — the rule the category standings use — so the
    /// two ratings are read and computed the same way.
    /// </summary>
    /// <param name="report"></param>
    /// <param name="group"></param>
    /// <param name="includeLibrary">
    /// Optional library filter; the web application passes the current selection.
    /// </param>
    /// <param name="isRated">
    /// Which libraries take part in the rating. A library outside it is still
    /// drawn — a hand-written baseline is the most useful row on the chart — but
    /// it scores nothing and does not define the best result, so the points here
    /// are measured against the same field as the category standings.
    /// </param>
    public static MatrixOverview? Create(
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
        var score = MatrixScores.Create(
            features,
            competitors.Select(library => library.Id),
            libraryId => (includeLibrary?.Invoke(libraryId) ?? true) && Rated(libraryId));

        // Rated libraries first, by score; the reference rows follow, ordered by
        // what they measured, because they have no score to be ordered by.
        var rows = libraries
            .Select(library => CreateRow(library, features, score, Rated(library.Id)))
            .OrderByDescending(row => row.Rated)
            .ThenByDescending(row => row.Rated ? row.Points : 0)
            .ThenBy(row => row.Rated ? 0 : MatrixMetrics.Total(row.PerformanceValues))
            .ThenBy(row => row.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        return new MatrixOverview(
            group,
            features,
            rows,
            rows.Select(row => MatrixMetrics.Total(row.PerformanceValues)).DefaultIfEmpty().Max(),
            rows.Select(row => MatrixMetrics.Total(row.MemoryValues)).DefaultIfEmpty().Max());

        bool Rated(string libraryId) => isRated?.Invoke(libraryId) ?? true;
    }

    private static MatrixOverviewRow CreateRow(
        BenchmarkLibrary library,
        IReadOnlyList<BenchmarkReportEntry> features,
        IReadOnlyDictionary<string, MatrixScore> score,
        bool rated)
    {
        var results = features
            .Select(feature => feature.Results.FirstOrDefault(result =>
                result.Successful
                && result.LibraryId.Equals(library.Id, StringComparison.OrdinalIgnoreCase)))
            .ToArray();
        var earned = score.GetValueOrDefault(library.Id) ?? new MatrixScore(0, 0, 0);
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
}
