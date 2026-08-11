using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FirstMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public Order Hyperlinq()
    {
        var result = _source.AsValueEnumerable().First(static o => o.Amount >= 10_000);
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
