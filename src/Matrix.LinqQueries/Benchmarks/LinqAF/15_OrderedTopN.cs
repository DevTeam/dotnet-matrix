using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class OrderedTopN
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public int[] LinqAF()
    {
        var result = _source.OrderByDescending(static o => o.Amount).Take(20).Select(static o => o.Id).ToArray();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
