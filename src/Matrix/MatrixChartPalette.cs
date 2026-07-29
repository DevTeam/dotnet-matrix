using System.Diagnostics.CodeAnalysis;

namespace Matrix;

/// <summary>
/// Per-feature series colours, shared so that the readme PNG and the interactive
/// chart in the web application colour the same scenario the same way.
/// </summary>
public static class MatrixChartPalette
{
    public static readonly IReadOnlyList<string> Features =
    [
        "#68D8EF",
        "#B7F34A",
        "#B86BE3",
        "#FF7B7F",
        "#F4BD50",
        "#57D6B9",
        "#7F9CFF",
        "#E78CC8"
    ];

    [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
    public static string Feature(int index) =>
        Features[((index % Features.Count) + Features.Count) % Features.Count];
}