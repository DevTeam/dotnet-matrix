using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FirstMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public Order ZLinq()
    {
        var result = _source.AsValueEnumerable().First(static o => o.Amount >= 10_000);
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
