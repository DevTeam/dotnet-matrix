using Mapster;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class PrepareAndSimpleMap
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    public SimpleDestination Mapster()
    {
        var configuration = MapsterFactory.CreateConfiguration();
        var destination = _source.Adapt<SimpleDestination>(configuration);
        Validate(LibraryCatalog.Mapster, destination);
        return destination;
    }
}
