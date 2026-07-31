using TinyCsvParser.Models;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class HeaderMapping
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.TinyCsvParser)]
    public CsvRecord[] TinyCsvParser()
    {
        var records = new List<CsvRecord>(3);
        foreach (CsvMappingResult<CsvRecord> record in
                 TinyCsvParserConfiguration.Records.ReadFromString(_csv))
        {
            records.Add(record.Result);
        }

        var result = records.ToArray();
        CsvChecks.Records(LibraryCatalog.TinyCsvParser, result);
        return result;
    }
}
