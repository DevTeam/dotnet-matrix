using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterProjectToList
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public List<int> SystemLinq()
    {
        var result = _source.Where(static o => o.Amount > 2500).Select(static o => o.Id).ToList();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
