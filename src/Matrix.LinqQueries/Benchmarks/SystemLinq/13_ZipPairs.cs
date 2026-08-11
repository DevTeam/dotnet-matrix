using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ZipPairs
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public int[] SystemLinq()
    {
        var result = _first.Zip(_second, static (first, second) => first * second).ToArray();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
