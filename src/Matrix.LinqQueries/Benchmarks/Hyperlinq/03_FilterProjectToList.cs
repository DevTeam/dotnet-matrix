using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterProjectToList
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public List<int> Hyperlinq()
    {
        var result = _source.AsValueEnumerable().Where(static o => o.Amount > 2500).Select(static o => o.Id).ToList();
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
