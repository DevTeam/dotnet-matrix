using Mapster;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class NestedObject
{
    private TypeAdapterConfig _mapster = null!;

    public void SetupMapster() => _mapster = MapsterFactory.CreateConfiguration();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    public OrderDestination Mapster()
    {
        var destination = _source.Adapt<OrderDestination>(_mapster);
        Validate(LibraryCatalog.Mapster, destination);
        return destination;
    }
}
