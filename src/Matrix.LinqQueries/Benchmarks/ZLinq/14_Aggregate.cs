using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class Aggregate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public int ZLinq()
    {
        var result = _source.AsValueEnumerable().Aggregate(0, static (sum, value) => sum + value);
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
