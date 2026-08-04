// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

/// <summary>
/// One medal: the overview group it was won in and the place, 1 to 3.
/// </summary>
public sealed record MatrixMedal(
    string GroupId,
    string GroupName,
    int Place);