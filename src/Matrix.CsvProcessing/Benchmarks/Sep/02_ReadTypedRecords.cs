using nietras.SeparatedValues;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadTypedRecords
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sep)]
    public CsvRecord[] Sep()
    {
        using var csv = SepConfiguration.Reader.FromText(_csv);
        var records = new List<CsvRecord>(3);
        foreach (var row in csv)
        {
            records.Add(new CsvRecord(
                row[0].Parse<int>(),
                row[1].ToString(),
                row[2].Parse<decimal>(),
                row[3].Parse<bool>()));
        }

        var result = records.ToArray();
        CsvChecks.Records(LibraryCatalog.Sep, result);
        return result;
    }
}

