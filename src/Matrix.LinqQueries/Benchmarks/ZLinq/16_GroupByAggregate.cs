using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class GroupByAggregate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public RegionTotal[] ZLinq()
    {
        var result = _source.AsValueEnumerable().GroupBy(static o => o.Region).Select(static group => new RegionTotal(group.Key, group.AsValueEnumerable().Sum(static o => o.Amount))).ToArray();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
