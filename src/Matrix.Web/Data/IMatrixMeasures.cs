// ReSharper disable CheckNamespace
namespace Matrix.Web;

/// <summary>
/// A measured number on its way to the screen: written as text, summed, or turned
/// into the length of a bar. The build renders the same reports to PNG through the
/// same computation, so a result never reads differently in the two places.
/// </summary>
internal interface IMatrixMeasures
{
    /// <summary>Nanoseconds or bytes, in the unit that suits the magnitude.</summary>
    string Format(double value, bool memory);

    string FormatTime(double nanoseconds);

    /// <summary>How many times behind the best result, as `1.00x`.</summary>
    string Ratio(double current, double minimum);

    double Total(IReadOnlyList<double?> values);

    bool HasValues(IReadOnlyList<double?> values);

    /// <summary>
    /// The length of a bar as a percentage of its track. Logarithmic and measured
    /// against the maximum, so a longer bar always means a worse result and an
    /// outlier does not flatten everything else to a sliver.
    /// </summary>
    double ScalePercent(double current, double maximum);
}
