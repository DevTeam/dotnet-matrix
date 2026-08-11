using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class OrderedTopN
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public int[] ZLinq()
    {
        var result = _source.AsValueEnumerable().OrderByDescending(static o => o.Amount).Take(20).Select(static o => o.Id).ToArray();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
