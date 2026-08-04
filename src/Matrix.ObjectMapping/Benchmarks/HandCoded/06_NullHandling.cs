// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class NullHandling
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public NullableDestination HandCoded()
    {
        var destination = new NullableDestination
        {
            Text = _source.Text,
            Address = null,
            Items = null
        };
        Validate(LibraryCatalog.HandCoded, destination);
        return destination;
    }
}
