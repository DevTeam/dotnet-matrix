using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterCount
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public int ZLinq()
    {
        var result = _source.AsValueEnumerable().Where(static n => n % 3 == 0).Count();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
