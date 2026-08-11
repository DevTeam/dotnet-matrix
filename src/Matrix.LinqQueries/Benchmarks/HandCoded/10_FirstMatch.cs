// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FirstMatch
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public Order HandCoded()
    {
        Order? result = null;
        for (var i = 0; i < _source.Length; i++)
        {
            if (_source[i].Amount >= 10_000)
            {
                result = _source[i];
                break;
            }
        }

        if (result is null)
        {
            throw new InvalidOperationException("No matching order.");
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
