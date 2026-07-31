using TinyCsvParser.Models;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadSimpleRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.TinyCsvParser)]
    public RawCsvRow[] TinyCsvParser()
    {
        var rows = new List<RawCsvRow>(3);
        foreach (CsvMappingResult<RawCsvRow> row in
                 TinyCsvParserConfiguration.Raw.ReadFromString(_csv))
        {
            rows.Add(row.Result);
        }

        var result = rows.ToArray();
        CsvChecks.RawRows(LibraryCatalog.TinyCsvParser, result);
        return result;
    }
}
