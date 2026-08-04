// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class NestedObject
{
    private readonly MapperlyMapper _mapperly = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapperly)]
    public OrderDestination Mapperly()
    {
        var destination = _mapperly.MapOrder(_source);
        Validate(LibraryCatalog.Mapperly, destination);
        return destination;
    }
}
