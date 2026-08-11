using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FirstMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public Order LinqAF()
    {
        var result = _source.First(static o => o.Amount >= 10_000);
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
