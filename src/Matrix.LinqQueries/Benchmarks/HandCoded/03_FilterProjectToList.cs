// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterProjectToList
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public List<int> HandCoded()
    {
        var result = new List<int>();
        for (var i = 0; i < _source.Length; i++)
        {
            var order = _source[i];
            if (order.Amount > 2500)
            {
                result.Add(order.Id);
            }
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
