using CsvHelper;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class CustomConversion
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public ProductCode[] CsvHelper()
    {
        using var csv = CsvReader.Create(new StringReader(_csv), CsvHelperConfiguration.Reader);
        var codes = new List<ProductCode>(2);
        while (csv.Read())
        {
            codes.Add(csv.Parse<ProductCode>("Code"));
        }

        var result = codes.ToArray();
        CsvChecks.ProductCodes(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

