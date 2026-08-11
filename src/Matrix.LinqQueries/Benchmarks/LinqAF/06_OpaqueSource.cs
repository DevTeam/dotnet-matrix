using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class OpaqueSource
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public int[] LinqAF()
    {
        var result = _source().Where(static n => n % 3 == 0).Select(static n => n * 2).ToArray();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
