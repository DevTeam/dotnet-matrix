using AutoMapper;
// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class PolymorphicMapping
{
    private IMapper _autoMapper = null!;

    [GlobalSetup(Target = nameof(AutoMapper))]
    public void SetupAutoMapper() => _autoMapper = AutoMapperFactory.CreateMapper();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.AutoMapper)]
    public AnimalDestination[] AutoMapper()
    {
        var destination = _autoMapper.Map<AnimalDestination[]>(_source);
        Validate(LibraryCatalog.AutoMapper, destination);
        return destination;
    }
}
