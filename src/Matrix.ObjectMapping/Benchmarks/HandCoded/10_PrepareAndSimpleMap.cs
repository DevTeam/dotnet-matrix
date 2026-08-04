// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class PrepareAndSimpleMap
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public SimpleDestination HandCoded()
    {
        var destination = SimpleObject.Map(_source);
        Validate(LibraryCatalog.HandCoded, destination);
        return destination;
    }
}
