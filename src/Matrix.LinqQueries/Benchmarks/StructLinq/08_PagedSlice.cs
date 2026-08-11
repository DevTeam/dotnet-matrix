using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class PagedSlice
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int[] StructLinq()
    {
        var result = _source.ToStructEnumerable().Skip(4000).Take(1000).ToArray();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
