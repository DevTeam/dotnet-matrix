// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class Flattening
{
    private readonly MapperlyMapper _mapperly = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapperly)]
    public OrderSummaryDestination Mapperly()
    {
        var destination = _mapperly.MapSummary(_source);
        Validate(LibraryCatalog.Mapperly, destination);
        return destination;
    }
}
