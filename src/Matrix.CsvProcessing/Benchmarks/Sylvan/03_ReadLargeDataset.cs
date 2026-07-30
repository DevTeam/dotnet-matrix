using Sylvan.Data.Csv;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadLargeDataset
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sylvan)]
    public CsvRecord[] Sylvan()
    {
        using var source = new StringReader(_csv);
        using var csv = CsvDataReader.Create(source, SylvanConfiguration.Reader);
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
        CsvChecks.Large(LibraryCatalog.Sylvan, result);
        return result;
    }
}

