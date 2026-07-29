using Mapster;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class PolymorphicMapping
{
    private TypeAdapterConfig _mapster = null!;

    [GlobalSetup(Target = nameof(Mapster))]
    public void SetupMapster() => _mapster = MapsterFactory.CreateConfiguration();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    public AnimalDestination[] Mapster()
    {
        var destination = _source.Adapt<AnimalDestination[]>(_mapster);
        Validate(LibraryCatalog.Mapster, destination);
        return destination;
    }
}
