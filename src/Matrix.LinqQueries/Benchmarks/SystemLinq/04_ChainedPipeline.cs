using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ChainedPipeline
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public int[] SystemLinq()
    {
        var result = _source.Where(static n => n % 3 == 0).Select(static n => n * 2).Where(static n => n % 4 == 0).Take(1000).ToArray();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
