using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class PagedSlice
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public int[] Hyperlinq()
    {
        var result = _source.AsValueEnumerable().Skip(4000).Take(1000).ToArray();
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
