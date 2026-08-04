// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class Collection
{
    private readonly MapperlyMapper _mapperly = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapperly)]
    public SimpleDestination[] Mapperly()
    {
        var destination = _mapperly.MapCollection(_source);
        Validate(LibraryCatalog.Mapperly, destination);
        return destination;
    }
}
