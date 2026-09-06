using System.Globalization;

namespace Matrix;

/// <summary>
/// How a benchmark number becomes a bar and a caption. Shared by the build, which
/// renders reports to PNG for the readme, and by the web application, so that the
/// same result never reads differently in the two places.
/// </summary>
public static class MatrixMetrics
{
    /// <summary>
    /// The bar width the readme renderer uses. The web application scales its own
    /// bars through the same value so that the lower bound of <see cref="Scale"/>
    /// lands on the same fraction of the track.
    /// </summary>
    private const double BarWidth = 330;

    public static double Total(IReadOnlyList<double?> values) =>
        values.Sum(value => value ?? 0);

    public static bool HasValues(IReadOnlyList<double?> values) =>
        values.Any(value => value is not null);

    /// <summary>
    /// Logarithmic and measured against the maximum, so a longer bar always means
    /// a worse result and an outlier does not flatten everything else to a sliver.
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

    /// <summary>
    /// <see cref="Scale"/> expressed as a percentage of the track.
    /// </summary>
    public static double ScalePercent(double current, double maximum) =>
        Scale(current, maximum, BarWidth) / BarWidth * 100;

    /// <summary>
    /// Invariant throughout, for the same reason the scores are: the readme and the
    /// charts are rendered on whatever machine runs the build, and a decimal comma
    /// there would make `1,07 ms` read as a thousand and would disagree with the
    /// application, which formats the same result for a reader anywhere.
    /// </summary>
    private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

    public static string Ratio(double current, double minimum) =>
        minimum > 0
            ? string.Format(Culture, "{0:0.00}x", current / minimum)
            : current > 0
                ? "∞x"
                : "1.00x";

    public static string FormatTime(double nanoseconds) => nanoseconds switch
    {
        0 => "0 ns",
        < 1_000 => string.Format(Culture, "{0:0.##} ns", nanoseconds),
        < 1_000_000 => string.Format(Culture, "{0:0.##} μs", nanoseconds / 1_000),
        _ => string.Format(Culture, "{0:0.##} ms", nanoseconds / 1_000_000)
    };

    public static string FormatBytes(double bytes) => bytes switch
    {
        0 => "0 B",
        < 1_024 => string.Format(Culture, "{0:0.##} B", bytes),
        < 1_048_576 => string.Format(Culture, "{0:0.##} KB", bytes / 1_024),
        _ => string.Format(Culture, "{0:0.##} MB", bytes / 1_048_576)
    };

    public static string Format(double value, bool memory) =>
        memory ? FormatBytes(value) : FormatTime(value);
}
