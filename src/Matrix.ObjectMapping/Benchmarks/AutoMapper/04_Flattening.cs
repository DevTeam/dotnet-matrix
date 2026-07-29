using AutoMapper;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class Flattening
{
    private IMapper _autoMapper = null!;

    public void SetupAutoMapper() => _autoMapper = AutoMapperFactory.CreateMapper();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    public OrderSummaryDestination AutoMapper()
    {
        var destination = _autoMapper.Map<OrderSummaryDestination>(_source);
        Validate(LibraryCatalog.AutoMapper, destination);
        return destination;
    }
}
