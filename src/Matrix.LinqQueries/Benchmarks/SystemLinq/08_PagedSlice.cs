using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class PagedSlice
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public int[] SystemLinq()
    {
        var result = _source.Skip(4000).Take(1000).ToArray();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
