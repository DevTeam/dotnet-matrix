// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

/// <summary>
/// One library in a benchmark overview: its per-feature values in the order of
/// <see cref="MatrixOverview.Features"/>, plus the features it has no result for.
/// </summary>
public sealed record MatrixOverviewRow(
    string LibraryId,
    string Name,
    IReadOnlyList<double?> PerformanceValues,
    IReadOnlyList<double?> MemoryValues,
    IReadOnlyList<string> MissingFeatures);

/// <summary>
/// The data behind one overview chart. Shared by the build, which renders it to a
/// PNG for the readme, and by the web application, which renders it as markup.
/// </summary>
public sealed record MatrixOverview(
    MatrixChartGroup Group,
    IReadOnlyList<BenchmarkReportEntry> Features,
    IReadOnlyList<MatrixOverviewRow> Ranked,
    IReadOnlyList<MatrixOverviewRow> Unranked,
    double MaximumTime,
    double MaximumMemory);

public static class MatrixOverviews
{
    /// <summary>
    /// Returns null when the group names no feature present in the report.
    /// </summary>
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
            .OrderBy(row => Total(row.PerformanceValues))
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
            scaleRows.Select(row => Total(row.PerformanceValues)).DefaultIfEmpty().Max(),
            scaleRows.Select(row => Total(row.MemoryValues)).DefaultIfEmpty().Max());
    }

    public static double Total(IReadOnlyList<double?> values) =>
        values.Sum(value => value ?? 0);

    public static bool HasValues(IReadOnlyList<double?> values) =>
        values.Any(value => value is not null);

    /// <summary>
    /// Logarithmic, so that containers within the same order of magnitude stay
    /// comparable while an outlier does not flatten everything else to a sliver.
    /// The 7 unit floor keeps a non-zero result visible.
    /// </summary>
    public static double Scale(double current, double maximum, double width)
    {
        if (current <= 0)
        {
            return 7;
        }

        if (maximum <= 0)
        {
            return width;
        }

        return Math.Min(
            width,
            Math.Max(7, Math.Log10(current + 1) / Math.Log10(maximum + 1) * width));
    }

    public static string FormatTime(double nanoseconds) => nanoseconds switch
    {
        0 => "0 ns",
        < 1_000 => $"{nanoseconds:0.##} ns",
        < 1_000_000 => $"{nanoseconds / 1_000:0.##} μs",
        _ => $"{nanoseconds / 1_000_000:0.##} ms"
    };

    public static string FormatBytes(double bytes) => bytes switch
    {
        0 => "0 B",
        < 1_024 => $"{bytes:0.##} B",
        < 1_048_576 => $"{bytes / 1_024:0.##} KB",
        _ => $"{bytes / 1_048_576:0.##} MB"
    };

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
