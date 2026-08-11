using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class DistinctValues
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int[] StructLinq()
    {
        var result = _source.ToStructEnumerable().Distinct().ToArray();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
