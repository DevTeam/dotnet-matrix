using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class DistinctValues
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public int[] SystemLinq()
    {
        var result = _source.Distinct().ToArray();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
