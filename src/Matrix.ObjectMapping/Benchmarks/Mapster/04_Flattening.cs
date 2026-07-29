using Mapster;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class Flattening
{
    private TypeAdapterConfig _mapster = null!;

    public void SetupMapster() => _mapster = MapsterFactory.CreateConfiguration();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    public OrderSummaryDestination Mapster()
    {
        var destination = _source.Adapt<OrderSummaryDestination>(_mapster);
        Validate(LibraryCatalog.Mapster, destination);
        return destination;
    }
}
