using AutoMapper;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class PrepareConfiguration
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    public MapperConfiguration AutoMapper() => AutoMapperFactory.CreateConfiguration();
}
