using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class DistinctValues
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public int[] Hyperlinq()
    {
        var result = _source.AsValueEnumerable().Distinct().ToArray();
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
