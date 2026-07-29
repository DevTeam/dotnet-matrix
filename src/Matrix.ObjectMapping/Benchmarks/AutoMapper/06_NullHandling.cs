using AutoMapper;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class NullHandling
{
    private IMapper _autoMapper = null!;

    [GlobalSetup(Target = nameof(AutoMapper))]
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
