using nietras.SeparatedValues;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class QuotedFields
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
        CsvChecks.Quoted(LibraryCatalog.Sep, result);
        return result;
    }
}

