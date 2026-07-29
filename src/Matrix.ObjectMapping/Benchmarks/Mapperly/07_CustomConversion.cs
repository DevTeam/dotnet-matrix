namespace Matrix.ObjectMapping.Benchmarks;

public partial class CustomConversion
{
    private readonly MapperlyMapper _mapperly = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapperly)]
    public ConversionDestination Mapperly()
    {
        var destination = _mapperly.MapConversion(_source);
        Validate(LibraryCatalog.Mapperly, destination);
        return destination;
    }
}
