using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ProjectToArray
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public int[] SystemLinq()
    {
        var result = _source.Select(static n => n * 2).ToArray();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
