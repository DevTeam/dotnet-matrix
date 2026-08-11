using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ProjectToArray
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public int[] ZLinq()
    {
        var result = _source.AsValueEnumerable().Select(static n => n * 2).ToArray();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
