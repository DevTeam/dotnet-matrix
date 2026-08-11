using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class SpanSource
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public int[] ZLinq()
    {
        var result = ((ReadOnlySpan<int>)_source).AsValueEnumerable().Where(static n => n % 3 == 0).Select(static n => n * 2).ToArray();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
