using Sylvan.Data.Csv;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadTypedRecords
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sylvan)]
    public CsvRecord[] Sylvan()
    {
        using var source = new StringReader(_csv);
        using var csv = CsvDataReader.Create(source, SylvanConfiguration.Reader);
        var records = new List<CsvRecord>(3);
        while (csv.Read())
        {
            records.Add(new CsvRecord(
                csv.GetInt32(0),
                csv.GetString(1),
                csv.GetDecimal(2),
                csv.GetBoolean(3)));
        }

        var result = records.ToArray();
        CsvChecks.Records(LibraryCatalog.Sylvan, result);
        return result;
    }
}

