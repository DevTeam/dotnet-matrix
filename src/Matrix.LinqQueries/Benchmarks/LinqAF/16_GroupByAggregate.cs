using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class GroupByAggregate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public RegionTotal[] LinqAF()
    {
        var result = _source.GroupBy(static o => o.Region).Select(static group => new RegionTotal(group.Key, group.Sum(static o => o.Amount))).ToArray();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
