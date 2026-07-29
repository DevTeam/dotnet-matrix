namespace Matrix.ObjectMapping.Benchmarks;

public partial class SimpleObject
{
    private readonly MapperlyMapper _mapperly = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapperly)]
    public SimpleDestination Mapperly()
    {
        var destination = _mapperly.MapSimple(_source);
        Validate(LibraryCatalog.Mapperly, destination);
        return destination;
    }
}
