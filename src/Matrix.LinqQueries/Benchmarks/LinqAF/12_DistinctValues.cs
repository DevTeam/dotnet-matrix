using LinqAF;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.LinqQueries.Benchmarks;

public partial class DistinctValues
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public int[] LinqAF()
    {
        var result = _source.Distinct().ToArray();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
