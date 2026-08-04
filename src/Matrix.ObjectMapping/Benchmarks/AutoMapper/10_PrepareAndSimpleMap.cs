// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class PrepareAndSimpleMap
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    public SimpleDestination AutoMapper()
    {
        var mapper = AutoMapperFactory.CreateMapper();
        var destination = mapper.Map<SimpleDestination>(_source);
        Validate(LibraryCatalog.AutoMapper, destination);
        return destination;
    }
}
