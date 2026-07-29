using AutoMapper;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class NestedObject
{
    private IMapper _autoMapper = null!;

    public void SetupAutoMapper() => _autoMapper = AutoMapperFactory.CreateMapper();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    public OrderDestination AutoMapper()
    {
        var destination = _autoMapper.Map<OrderDestination>(_source);
        Validate(LibraryCatalog.AutoMapper, destination);
        return destination;
    }
}
