// ReSharper disable CheckNamespace
namespace Matrix.Web;

/// <inheritdoc cref="IMatrixPalette"/>
internal sealed class MatrixPalette : IMatrixPalette
{
    public string Feature(int index) => MatrixChartPalette.Feature(index);
}
