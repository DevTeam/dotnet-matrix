using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class AnyMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public bool SystemLinq()
    {
        var result = _source.Any(static n => n >= 700);
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
