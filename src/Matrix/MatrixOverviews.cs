namespace Matrix;

public static class MatrixOverviews
{
    /// <summary>
    /// Returns null when the group names no feature present in the report.
    /// </summary>
    /// <param name="report"></param>
    /// <param name="group"></param>
    /// <param name="includeLibrary">
    /// Optional library filter; the web application passes the current selection.
    /// </param>
    public static MatrixOverview? Create(
        BenchmarkReport report,
        MatrixChartGroup group,
        Func<string, bool>? includeLibrary = null)
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

        var rows = report.Libraries
            .Where(library => includeLibrary?.Invoke(library.Id) ?? true)
            .Select(library => CreateRow(library, features))
            .ToArray();
        var ranked = rows
            .Where(row => row.MissingFeatures.Count == 0)
            .OrderBy(row => MatrixMetrics.Total(row.PerformanceValues))
            .ThenBy(row => row.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var unranked = rows
            .Where(row => row.MissingFeatures.Count > 0)
            .OrderBy(row => row.MissingFeatures.Count)
            .ThenBy(row => row.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var scaleRows = ranked.Length > 0 ? ranked : unranked;
        return new MatrixOverview(
            group,
            features,
            ranked,
            unranked,
            scaleRows.Select(row => MatrixMetrics.Total(row.PerformanceValues)).DefaultIfEmpty().Max(),
            scaleRows.Select(row => MatrixMetrics.Total(row.MemoryValues)).DefaultIfEmpty().Max());
    }

    private static MatrixOverviewRow CreateRow(
        BenchmarkLibrary library,
        IReadOnlyList<BenchmarkReportEntry> features)
    {
        var results = features
            .Select(feature => feature.Results.FirstOrDefault(result =>
                result.Successful
                && result.LibraryId.Equals(library.Id, StringComparison.OrdinalIgnoreCase)))
            .ToArray();
        return new MatrixOverviewRow(
            library.Id,
            library.Name,
            results.Select(result => result?.MeanNanoseconds).ToArray(),
            results.Select(result => result?.AllocatedBytesPerOperation).ToArray(),
            features
                .Where((_, index) => results[index] is null)
                .Select(feature => feature.Name)
                .ToArray());
    }
}