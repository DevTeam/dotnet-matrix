using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class DistinctValues
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public int[] ZLinq()
    {
        var result = _source.AsValueEnumerable().Distinct().ToArray();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
