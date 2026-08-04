using CsvHelper;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class CustomConversion
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public ProductCode[] CsvHelper()
    {
        using var source = new StringReader(_csv);
        using var csv = new CsvReader(source, CsvHelperConfiguration.Reader);
        csv.Context.TypeConverterCache.AddConverter<ProductCode>(new ProductCodeConverter());
        csv.Read();
        csv.ReadHeader();
        var codes = new List<ProductCode>(2);
        while (csv.Read())
        {
            codes.Add(csv.GetField<ProductCode>("Code"));
        }

        var result = codes.ToArray();
        CsvChecks.ProductCodes(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

