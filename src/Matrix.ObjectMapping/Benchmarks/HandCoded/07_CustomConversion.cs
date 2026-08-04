using System.Globalization;
// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class CustomConversion
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public ConversionDestination HandCoded()
    {
        var destination = new ConversionDestination
        {
            Code = new MappingCode(_source.Code),
            Amount = decimal.Parse(_source.Amount, CultureInfo.InvariantCulture)
        };
        Validate(LibraryCatalog.HandCoded, destination);
        return destination;
    }
}
