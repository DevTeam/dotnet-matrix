using CsvHelper;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class HeaderMapping
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public CsvRecord[] CsvHelper()
    {
        using var csv = CsvReader.Create(new StringReader(_csv), CsvHelperConfiguration.Reader);
        var id = csv.GetFieldIndex("Id");
        var name = csv.GetFieldIndex("Name");
        var amount = csv.GetFieldIndex("Amount");
        var active = csv.GetFieldIndex("Active");
        var records = new List<CsvRecord>(3);
        while (csv.Read())
        {
            records.Add(new CsvRecord(
                csv.Parse<int>(id),
                csv.GetString(name),
                csv.Parse<decimal>(amount),
                csv.Parse<bool>(active)));
        }

        var result = records.ToArray();
        CsvChecks.Records(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

