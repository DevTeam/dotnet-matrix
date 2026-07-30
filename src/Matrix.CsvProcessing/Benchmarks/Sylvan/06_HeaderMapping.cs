using Sylvan.Data.Csv;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class HeaderMapping
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sylvan)]
    public CsvRecord[] Sylvan()
    {
        using var source = new StringReader(_csv);
        using var csv = CsvDataReader.Create(source, SylvanConfiguration.Reader);
        var id = csv.GetOrdinal("Id");
        var name = csv.GetOrdinal("Name");
        var amount = csv.GetOrdinal("Amount");
        var active = csv.GetOrdinal("Active");
        var records = new List<CsvRecord>(3);
        while (csv.Read())
        {
            records.Add(new CsvRecord(
                csv.GetInt32(id),
                csv.GetString(name),
                csv.GetDecimal(amount),
                csv.GetBoolean(active)));
        }

        var result = records.ToArray();
        CsvChecks.Records(LibraryCatalog.Sylvan, result);
        return result;
    }
}

