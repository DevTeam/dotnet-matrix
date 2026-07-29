using AutoMapper;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class Collection
{
    private IMapper _autoMapper = null!;

    [GlobalSetup(Target = nameof(AutoMapper))]
    public void SetupAutoMapper() => _autoMapper = AutoMapperFactory.CreateMapper();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    public SimpleDestination[] AutoMapper()
    {
        var destination = _autoMapper.Map<SimpleDestination[]>(_source);
        Validate(LibraryCatalog.AutoMapper, destination);
        return destination;
    }
}
