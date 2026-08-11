using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class AnyMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public bool LinqAF()
    {
        var result = _source.Any(static n => n >= 700);
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
