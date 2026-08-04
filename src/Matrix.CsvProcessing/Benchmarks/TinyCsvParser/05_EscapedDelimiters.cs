// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class EscapedDelimiters
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.TinyCsvParser)]
    public TextCsvRow[] TinyCsvParser()
    {
        var rows = new List<TextCsvRow>(2);
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var row in TinyCsvParserConfiguration.Text.ReadFromString(_csv))
        {
            rows.Add(row.Result);
        }

        var result = rows.ToArray();
        CsvChecks.EscapedDelimiters(LibraryCatalog.TinyCsvParser, result);
        return result;
    }
}
