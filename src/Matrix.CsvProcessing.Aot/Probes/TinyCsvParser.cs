using TinyCsvParser;
using TinyCsvParser.Models;

namespace Matrix.CsvProcessing.Aot;

internal static class AotProbe
{
    public const string Library = "TinyCsvParser";

    public const int ExpectedEvents = 1;

    private const string Csv = "probe,7";

    /// <summary>
    /// Parses one data row from an in-memory CSV, exactly like the benchmarks' <c>ReadSimpleRows</c>
    /// scenario minus the shared fixture, and checks the parsed field values.
    /// </summary>
    public static int Run()
    {
        var options = new CsvOptions(Delimiter: ',', QuoteChar: '"', EscapeChar: '"', SkipHeader: false);
        var parser = new CsvParser<ProbeRow>(options, new ProbeRowMapping());
        foreach (var result in parser.ReadFromString(Csv))
        {
            return result.Result.Name == "probe" && result.Result.Value == 7 ? 1 : 0;
        }

        return 0;
    }

    private sealed class ProbeRow
    {
        public string Name { get; set; } = string.Empty;

        public int Value { get; set; }
    }

    private sealed class ProbeRowMapping : CsvMapping<ProbeRow>
    {
        public ProbeRowMapping()
        {
            MapProperty(0, row => row.Name);
            MapProperty(1, row => row.Value);
        }
    }
}
