using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FlattenSelectMany
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public int[] ZLinq()
    {
        var result = _source.AsValueEnumerable().SelectMany(static batch => batch.AsValueEnumerable()).ToArray();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
