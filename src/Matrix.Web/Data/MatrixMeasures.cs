// ReSharper disable CheckNamespace
namespace Matrix.Web;

/// <inheritdoc cref="IMatrixMeasures"/>
internal sealed class MatrixMeasures : IMatrixMeasures
{
    public string Format(double value, bool memory) => MatrixMetrics.Format(value, memory);

    public string FormatTime(double nanoseconds) => MatrixMetrics.FormatTime(nanoseconds);

    public string Ratio(double current, double minimum) => MatrixMetrics.Ratio(current, minimum);

    public double Total(IReadOnlyList<double?> values) => MatrixMetrics.Total(values);

    public bool HasValues(IReadOnlyList<double?> values) => MatrixMetrics.HasValues(values);

    public double ScalePercent(double current, double maximum) =>
        MatrixMetrics.ScalePercent(current, maximum);
}
