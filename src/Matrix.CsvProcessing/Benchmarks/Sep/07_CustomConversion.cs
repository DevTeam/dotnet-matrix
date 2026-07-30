using nietras.SeparatedValues;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class CustomConversion
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sep)]
    public ProductCode[] Sep()
    {
        using var csv = SepConfiguration.Reader.FromText(_csv);
        var codes = new List<ProductCode>(2);
        foreach (var row in csv)
        {
            codes.Add(row[0].Parse<ProductCode>());
        }

        var result = codes.ToArray();
        CsvChecks.ProductCodes(LibraryCatalog.Sep, result);
        return result;
    }
}

