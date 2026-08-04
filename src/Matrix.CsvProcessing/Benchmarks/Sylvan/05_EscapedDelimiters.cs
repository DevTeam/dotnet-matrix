using Sylvan.Data.Csv;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class EscapedDelimiters
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sylvan)]
    public TextCsvRow[] Sylvan()
    {
        using var source = new StringReader(_csv);
        using var csv = CsvDataReader.Create(source, SylvanConfiguration.Reader);
        var rows = new List<TextCsvRow>(2);
        while (csv.Read())
        {
            rows.Add(new TextCsvRow(csv.GetInt32(0), csv.GetString(1)));
        }

        var result = rows.ToArray();
        CsvChecks.EscapedDelimiters(LibraryCatalog.Sylvan, result);
        return result;
    }
}

