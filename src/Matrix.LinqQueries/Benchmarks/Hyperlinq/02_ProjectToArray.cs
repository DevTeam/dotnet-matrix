using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ProjectToArray
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public int[] Hyperlinq()
    {
        var result = _source.AsValueEnumerable().Select(static n => n * 2).ToArray();
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
