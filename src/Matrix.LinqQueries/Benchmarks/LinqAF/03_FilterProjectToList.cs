using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterProjectToList
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public List<int> LinqAF()
    {
        var result = _source.Where(static o => o.Amount > 2500).Select(static o => o.Id).ToList();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
