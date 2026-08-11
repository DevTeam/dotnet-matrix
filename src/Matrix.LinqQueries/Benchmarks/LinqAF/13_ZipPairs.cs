using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ZipPairs
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public int[] LinqAF()
    {
        var result = _first.Zip(_second, static (first, second) => first * second).ToArray();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
