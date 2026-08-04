// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

public partial class SimpleObject
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public SimpleDestination HandCoded()
    {
        var destination = Map(_source);
        Validate(LibraryCatalog.HandCoded, destination);
        return destination;
    }

    internal static SimpleDestination Map(SimpleSource source) =>
        new()
        {
            Id = source.Id,
            Name = source.Name,
            Amount = source.Amount,
            CreatedAt = source.CreatedAt,
            Active = source.Active
        };
}
