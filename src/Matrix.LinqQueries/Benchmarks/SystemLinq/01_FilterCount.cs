using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterCount
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public int SystemLinq()
    {
        var result = _source.Where(static n => n % 3 == 0).Count();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
