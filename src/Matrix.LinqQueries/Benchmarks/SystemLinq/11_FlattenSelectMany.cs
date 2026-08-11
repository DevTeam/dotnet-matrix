using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FlattenSelectMany
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public int[] SystemLinq()
    {
        var result = _source.SelectMany(static batch => batch).ToArray();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
