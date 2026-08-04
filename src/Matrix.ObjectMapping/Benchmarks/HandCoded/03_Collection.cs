// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class Collection
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public SimpleDestination[] HandCoded()
    {
        var destination = new SimpleDestination[_source.Length];
        for (var index = 0; index < _source.Length; index++)
        {
            destination[index] = SimpleObject.Map(_source[index]);
        }

        Validate(LibraryCatalog.HandCoded, destination);
        return destination;
    }
}
