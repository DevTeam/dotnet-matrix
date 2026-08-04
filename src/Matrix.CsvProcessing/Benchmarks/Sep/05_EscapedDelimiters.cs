using nietras.SeparatedValues;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class EscapedDelimiters
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sep)]
    public TextCsvRow[] Sep()
    {
        using var csv = SepConfiguration.Reader.FromText(_csv);
        var rows = new List<TextCsvRow>(2);
        foreach (var row in csv)
        {
            rows.Add(new TextCsvRow(row[0].Parse<int>(), row[1].ToString()));
        }

        var result = rows.ToArray();
        CsvChecks.EscapedDelimiters(LibraryCatalog.Sep, result);
        return result;
    }
}

