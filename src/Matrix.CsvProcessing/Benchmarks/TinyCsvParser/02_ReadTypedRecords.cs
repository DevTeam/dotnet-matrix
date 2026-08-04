// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadTypedRecords
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.TinyCsvParser)]
    public CsvRecord[] TinyCsvParser()
    {
        var records = new List<CsvRecord>(3);
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var record in TinyCsvParserConfiguration.Records.ReadFromString(_csv))
        {
            records.Add(record.Result);
        }

        var result = records.ToArray();
        CsvChecks.Records(LibraryCatalog.TinyCsvParser, result);
        return result;
    }
}
