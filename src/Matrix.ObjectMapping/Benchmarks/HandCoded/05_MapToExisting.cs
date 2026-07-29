namespace Matrix.ObjectMapping.Benchmarks;

public partial class MapToExisting
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public SimpleDestination HandCoded()
    {
        _destination.Id = _source.Id;
        _destination.Name = _source.Name;
        _destination.Amount = _source.Amount;
        _destination.CreatedAt = _source.CreatedAt;
        _destination.Active = _source.Active;
        Validate(LibraryCatalog.HandCoded, _destination);
        return _destination;
    }
}
