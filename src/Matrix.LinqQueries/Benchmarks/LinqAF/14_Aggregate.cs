using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class Aggregate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public int LinqAF()
    {
        var result = _source.Aggregate(0, static (sum, value) => sum + value);
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
