using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterCount
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public int LinqAF()
    {
        var result = _source.Where(static n => n % 3 == 0).Count();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
