using nietras.SeparatedValues;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class HeaderMapping
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sep)]
    public CsvRecord[] Sep()
    {
        using var csv = SepConfiguration.Reader.FromText(_csv);
        var id = csv.Header.IndexOf("Id");
        var name = csv.Header.IndexOf("Name");
        var amount = csv.Header.IndexOf("Amount");
        var active = csv.Header.IndexOf("Active");
        var records = new List<CsvRecord>(3);
        foreach (var row in csv)
        {
            records.Add(new CsvRecord(
                row[id].Parse<int>(),
                row[name].ToString(),
                row[amount].Parse<decimal>(),
                row[active].Parse<bool>()));
        }

        var result = records.ToArray();
        CsvChecks.Records(LibraryCatalog.Sep, result);
        return result;
    }
}

