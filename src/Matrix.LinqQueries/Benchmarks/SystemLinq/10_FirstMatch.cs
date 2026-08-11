using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FirstMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public Order SystemLinq()
    {
        var result = _source.First(static o => o.Amount >= 10_000);
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
