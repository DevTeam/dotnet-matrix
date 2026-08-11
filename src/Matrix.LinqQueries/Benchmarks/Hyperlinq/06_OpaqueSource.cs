using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class OpaqueSource
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public int[] Hyperlinq()
    {
        var result = _source().AsValueEnumerable<int>().Where(static n => n % 3 == 0).Select(static n => n * 2).ToArray();
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
