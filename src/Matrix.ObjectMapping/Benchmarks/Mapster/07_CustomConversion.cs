using Mapster;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class CustomConversion
{
    private TypeAdapterConfig _mapster = null!;

    [GlobalSetup(Target = nameof(Mapster))]
    public void SetupMapster() => _mapster = MapsterFactory.CreateConfiguration();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    public ConversionDestination Mapster()
    {
        var destination = _source.Adapt<ConversionDestination>(_mapster);
        Validate(LibraryCatalog.Mapster, destination);
        return destination;
    }
}
