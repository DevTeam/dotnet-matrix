using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ChainedPipeline
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public int[] Hyperlinq()
    {
        var result = _source.AsValueEnumerable().Where(static n => n % 3 == 0).Select(static n => n * 2).Where(static n => n % 4 == 0).Take(1000).ToArray();
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
