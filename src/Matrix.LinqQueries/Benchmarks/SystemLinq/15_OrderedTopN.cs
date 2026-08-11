using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class OrderedTopN
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public int[] SystemLinq()
    {
        var result = _source.OrderByDescending(static o => o.Amount).Take(20).Select(static o => o.Id).ToArray();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
