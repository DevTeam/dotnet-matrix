using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FlattenSelectMany
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public int[] LinqAF()
    {
        var result = _source.SelectMany(static batch => batch).ToArray();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
