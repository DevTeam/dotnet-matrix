using TinyCsvParser.Models;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadLargeDataset
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.TinyCsvParser)]
    public CsvRecord[] TinyCsvParser()
    {
        var records = new List<CsvRecord>(10_000);
        foreach (CsvMappingResult<CsvRecord> record in
                 TinyCsvParserConfiguration.Records.ReadFromString(_csv))
        {
            records.Add(record.Result);
        }

        var result = records.ToArray();
        CsvChecks.Large(LibraryCatalog.TinyCsvParser, result);
        return result;
    }
}
