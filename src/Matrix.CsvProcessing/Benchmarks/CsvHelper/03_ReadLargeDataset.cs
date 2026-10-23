using CsvHelper;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadLargeDataset
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public CsvRecord[] CsvHelper()
    {
        using var csv = CsvReader.Create(new StringReader(_csv), CsvHelperConfiguration.Reader);
        var records = new List<CsvRecord>(10_000);
        while (csv.Read())
        {
            records.Add(new CsvRecord(
                csv.GetInt32(0),
                csv.GetString(1),
                csv.GetDecimal(2),
                csv.GetBoolean(3)));
        }

        var result = records.ToArray();
        CsvChecks.Large(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

