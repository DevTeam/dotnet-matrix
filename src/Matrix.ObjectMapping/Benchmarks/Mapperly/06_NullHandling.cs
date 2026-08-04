// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class NullHandling
{
    private readonly MapperlyMapper _mapperly = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapperly)]
    public NullableDestination Mapperly()
    {
        var destination = _mapperly.MapNullable(_source);
        Validate(LibraryCatalog.Mapperly, destination);
        return destination;
    }
}
