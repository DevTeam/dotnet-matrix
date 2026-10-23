using CsvHelper;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class QuotedFields
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public TextCsvRow[] CsvHelper()
    {
        using var csv = CsvReader.Create(new StringReader(_csv), CsvHelperConfiguration.Reader);
        var rows = new List<TextCsvRow>(2);
        while (csv.Read())
        {
            rows.Add(new TextCsvRow(csv.GetInt32(0), csv.GetString(1)));
        }

        var result = rows.ToArray();
        CsvChecks.Quoted(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

