using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ZipPairs
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public int[] ZLinq()
    {
        var result = _first.AsValueEnumerable().Zip(_second.AsValueEnumerable(), static (first, second) => first * second).ToArray();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
