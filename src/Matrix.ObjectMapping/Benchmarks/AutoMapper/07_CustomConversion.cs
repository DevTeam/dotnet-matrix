using AutoMapper;

namespace Matrix.ObjectMapping.Benchmarks;

public partial class CustomConversion
{
    private IMapper _autoMapper = null!;

    [GlobalSetup(Target = nameof(AutoMapper))]
    public void SetupAutoMapper() => _autoMapper = AutoMapperFactory.CreateMapper();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    public ConversionDestination AutoMapper()
    {
        var destination = _autoMapper.Map<ConversionDestination>(_source);
        Validate(LibraryCatalog.AutoMapper, destination);
        return destination;
    }
}
