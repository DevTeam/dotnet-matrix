namespace Matrix.ObjectMapping.Benchmarks;

public partial class PolymorphicMapping
{
    private readonly MapperlyMapper _mapperly = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Mapperly)]
    public AnimalDestination[] Mapperly()
    {
        var destination = _mapperly.MapAnimals(_source);
        Validate(LibraryCatalog.Mapperly, destination);
        return destination;
    }
}
