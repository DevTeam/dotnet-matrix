using Mapster;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class NullHandling
{
    private TypeAdapterConfig _mapster = null!;

    public void SetupMapster() => _mapster = MapsterFactory.CreateConfiguration();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    public NullableDestination Mapster()
    {
        var destination = _source.Adapt<NullableDestination>(_mapster);
        Validate(LibraryCatalog.Mapster, destination);
        return destination;
    }
}
