using AutoMapper;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class NullHandling
{
    private IMapper _autoMapper = null!;

    public void SetupAutoMapper() => _autoMapper = AutoMapperFactory.CreateMapper();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    public NullableDestination AutoMapper()
    {
        var destination = _autoMapper.Map<NullableDestination>(_source);
        Validate(LibraryCatalog.AutoMapper, destination);
        return destination;
    }
}
