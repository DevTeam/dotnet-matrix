using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class Aggregate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public int SystemLinq()
    {
        var result = _source.Aggregate(0, static (sum, value) => sum + value);
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
