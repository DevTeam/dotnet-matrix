// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class GroupByAggregate
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public RegionTotal[] HandCoded()
    {
        var totals = new Dictionary<string, int>(StringComparer.Ordinal);
        for (var i = 0; i < _source.Length; i++)
        {
            var order = _source[i];
            totals.TryGetValue(order.Region, out var total);
            totals[order.Region] = total + order.Amount;
        }

        var result = new RegionTotal[totals.Count];
        var index = 0;
        foreach (var pair in totals)
        {
            result[index++] = new RegionTotal(pair.Key, pair.Value);
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
