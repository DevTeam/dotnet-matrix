using System.Diagnostics.CodeAnalysis;
using System.Text;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Matrix;

public sealed record MatrixChartCatalog(
    int SchemaVersion,
    IReadOnlyList<MatrixChartGroup> Groups);

public sealed record MatrixChartGroup(
    string Id,
    string Name,
    IReadOnlyList<string> Features);

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

public static class MatrixChartPaths
{
    public const string DirectoryName = "charts";

    public static string Feature(BenchmarkReportEntry feature) =>
        $"{feature.Order:00}-{Slug(feature.Name)}.png";

    public static string Overview(MatrixChartGroup group) =>
        $"overview-{Slug(group.Id)}.png";

    private static string Slug(string value)
    {
        var result = new StringBuilder(value.Length);
        var separator = false;
        foreach (var character in value)
        {
            if (char.IsLetterOrDigit(character))
            {
                if (separator && result.Length > 0)
                {
                    result.Append('-');
                }

                result.Append(char.ToLowerInvariant(character));
                separator = false;
            }
            else
            {
                separator = true;
            }
        }

        return result.ToString();
    }
}
