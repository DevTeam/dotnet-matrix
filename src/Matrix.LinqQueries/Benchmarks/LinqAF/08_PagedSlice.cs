using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class PagedSlice
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public int[] LinqAF()
    {
        var result = _source.Skip(4000).Take(1000).ToArray();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
