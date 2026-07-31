using TinyCsvParser.Models;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class CustomConversion
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.TinyCsvParser)]
    public ProductCode[] TinyCsvParser()
    {
        var codes = new List<ProductCode>(2);
        foreach (CsvMappingResult<ProductCodeRow> row in
                 TinyCsvParserConfiguration.ProductCodes.ReadFromString(_csv))
        {
            codes.Add(row.Result.Code);
        }

        var result = codes.ToArray();
        CsvChecks.ProductCodes(LibraryCatalog.TinyCsvParser, result);
        return result;
    }
}
