// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class Aggregate
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int HandCoded()
    {
        var result = 0;
        for (var i = 0; i < _source.Length; i++)
        {
            result += _source[i];
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
