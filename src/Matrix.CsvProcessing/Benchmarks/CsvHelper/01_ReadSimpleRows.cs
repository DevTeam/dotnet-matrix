using CsvHelper;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadSimpleRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public RawCsvRow[] CsvHelper()
    {
        using var csv = CsvReader.Create(new StringReader(_csv), CsvHelperConfiguration.Reader);
        var rows = new List<RawCsvRow>(3);
        while (csv.Read())
        {
            rows.Add(new RawCsvRow(
                csv.GetString(0),
                csv.GetString(1),
                csv.GetString(2),
                csv.GetString(3)));
        }

        var result = rows.ToArray();
        CsvChecks.RawRows(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

