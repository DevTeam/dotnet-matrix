using Mapster;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class MapToExisting
{
    private TypeAdapterConfig _mapster = null!;

    [GlobalSetup(Target = nameof(Mapster))]
    public void SetupMapster() => _mapster = MapsterFactory.CreateConfiguration();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    public SimpleDestination Mapster()
    {
        _source.Adapt(_destination, _mapster);
        Validate(LibraryCatalog.Mapster, _destination);
        return _destination;
    }
}
