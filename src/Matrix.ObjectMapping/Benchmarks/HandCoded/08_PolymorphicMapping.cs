namespace Matrix.ObjectMapping.Benchmarks;

public partial class PolymorphicMapping
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public AnimalDestination[] HandCoded()
    {
        var destination = new AnimalDestination[_source.Length];
        for (var index = 0; index < _source.Length; index++)
        {
            destination[index] = _source[index] switch
            {
                CatSource cat => new CatDestination
                {
                    Name = cat.Name,
                    Lives = cat.Lives
                },
                DogSource dog => new DogDestination
                {
                    Name = dog.Name,
                    GoodBoy = dog.GoodBoy
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        Validate(LibraryCatalog.HandCoded, destination);
        return destination;
    }
}
