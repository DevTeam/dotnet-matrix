using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ProjectToArray
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public int[] LinqAF()
    {
        var result = _source.Select(static n => n * 2).ToArray();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
