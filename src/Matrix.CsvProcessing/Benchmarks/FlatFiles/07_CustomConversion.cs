// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class CustomConversion
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatFiles)]
    public ProductCode[] FlatFiles()
    {
        var reader = FlatFilesConfiguration.ProductCodeMapper.GetReader(
            new StringReader(_csv),
            FlatFilesConfiguration.Options);
        var codes = new List<ProductCode>(2);
        while (reader.Read())
        {
            codes.Add(reader.Current.Code);
        }

        var result = codes.ToArray();
        CsvChecks.ProductCodes(LibraryCatalog.FlatFiles, result);
        return result;
    }
}
