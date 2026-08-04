// ReSharper disable CheckNamespace
namespace Matrix.Web;

/// <summary>
/// The colour a scenario carries. Shared with the chart renderer, so a scenario is
/// the same colour in the application and in the images committed to the repository.
/// </summary>
internal interface IMatrixPalette
{
    string Feature(int index);
}
