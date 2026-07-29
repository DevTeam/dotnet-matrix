using Mapster;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class Collection
{
    private TypeAdapterConfig _mapster = null!;

    [GlobalSetup(Target = nameof(Mapster))]
    public void SetupMapster() => _mapster = MapsterFactory.CreateConfiguration();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    public SimpleDestination[] Mapster()
    {
        var destination = _source.Adapt<SimpleDestination[]>(_mapster);
        Validate(LibraryCatalog.Mapster, destination);
        return destination;
    }
}
