using NReco.Csv;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadSimpleRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NRecoCsv)]
    public RawCsvRow[] NRecoCsv()
    {
        var reader = new CsvReader(new StringReader(_csv), NRecoCsvConfiguration.Separator);
        reader.Read();
        var rows = new List<RawCsvRow>(3);
        while (reader.Read())
        {
            rows.Add(new RawCsvRow(reader[0], reader[1], reader[2], reader[3]));
        }

        var result = rows.ToArray();
        CsvChecks.RawRows(LibraryCatalog.NRecoCsv, result);
        return result;
    }
}
