using AutoMapper;
// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class MapToExisting
{
    private IMapper _autoMapper = null!;

    [GlobalSetup(Target = nameof(AutoMapper))]
    public void SetupAutoMapper() => _autoMapper = AutoMapperFactory.CreateMapper();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    public SimpleDestination AutoMapper()
    {
        var destination = _autoMapper.Map(_source, _destination);
        Validate(LibraryCatalog.AutoMapper, destination);
        return destination;
    }
}
