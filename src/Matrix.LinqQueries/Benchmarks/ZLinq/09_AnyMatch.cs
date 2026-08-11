using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class AnyMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public bool ZLinq()
    {
        var result = _source.AsValueEnumerable().Any(static n => n >= 700);
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
