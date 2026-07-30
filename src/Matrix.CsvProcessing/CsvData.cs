using System.Globalization;
using System.Reflection;
using System.Text;

namespace Matrix.CsvProcessing;

internal static class CsvData
{
    public const string WriteCsv = "Id,Name\n1,Ada\n2,Grace\n3,Linus\n";

    public static readonly string SimpleCsv = ReadFixture("simple.csv");
    public static readonly string QuotedCsv = ReadFixture("quoted.csv");
    public static readonly string EscapedDelimitersCsv = ReadFixture("escaped-delimiters.csv");
    public static readonly string ReorderedCsv = ReadFixture("reordered.csv");
    public static readonly string CustomConversionCsv = ReadFixture("custom-conversion.csv");
    public static readonly string LargeCsv = CreateLargeCsv();
    public static readonly CsvWriteRecord[] WriteRecords =
    [
        new(1, "Ada"),
        new(2, "Grace"),
        new(3, "Linus")
    ];

    public static CsvAggregate LargeAggregate { get; } =
        new(10_000, 50_005_000, 4_999_500m, 5_000);

    private static string ReadFixture(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"{assembly.GetName().Name}.Fixtures.{fileName}";
        using var stream = assembly.GetManifestResourceStream(resourceName)
                           ?? throw new InvalidOperationException(
                               $"Embedded CSV fixture '{resourceName}' was not found.");
        using var reader = new StreamReader(
            stream,
            new UTF8Encoding(false, true),
            true);
        return NormalizeNewLines(reader.ReadToEnd());
    }

    private static string CreateLargeCsv()
    {
        var result = new StringBuilder(320_000);
        result.Append("Id,Name,Amount,Active\n");
        for (var id = 1; id <= 10_000; id++)
        {
            result.Append(id);
            result.Append(",Name");
            result.Append(id);
            result.Append(',');
            result.Append(((id - 1) / 10m).ToString("0.0", CultureInfo.InvariantCulture));
            result.Append(',');
            result.Append(id % 2 == 0 ? "true\n" : "false\n");
        }

        return result.ToString();
    }

    private static string NormalizeNewLines(string value) =>
        value.Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n');
}

