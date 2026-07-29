namespace Matrix.ObjectMapping.Benchmarks;

public partial class MapToExisting
{
    private readonly MapperlyMapper _mapperly = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapperly)]
    public SimpleDestination Mapperly()
    {
        _mapperly.MapExisting(_source, _destination);
        Validate(LibraryCatalog.Mapperly, _destination);
        return _destination;
    }
}
