using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterProjectToList
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public List<int> ZLinq()
    {
        var result = _source.AsValueEnumerable().Where(static o => o.Amount > 2500).Select(static o => o.Id).ToList();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
