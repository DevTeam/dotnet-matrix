using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class PagedSlice
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public int[] ZLinq()
    {
        var result = _source.AsValueEnumerable().Skip(4000).Take(1000).ToArray();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
