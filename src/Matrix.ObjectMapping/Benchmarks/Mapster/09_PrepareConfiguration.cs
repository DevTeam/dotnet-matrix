using Mapster;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class PrepareConfiguration
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapster)]
    public TypeAdapterConfig Mapster() => MapsterFactory.CreateConfiguration();
}
