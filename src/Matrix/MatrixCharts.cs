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
