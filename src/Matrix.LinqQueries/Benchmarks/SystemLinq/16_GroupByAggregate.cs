using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class GroupByAggregate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public RegionTotal[] SystemLinq()
    {
        var result = _source.GroupBy(static o => o.Region).Select(static group => new RegionTotal(group.Key, group.Sum(static o => o.Amount))).ToArray();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
