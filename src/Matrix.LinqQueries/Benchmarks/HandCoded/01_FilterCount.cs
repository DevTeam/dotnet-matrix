// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterCount
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int HandCoded()
    {
        var count = 0;
        for (var i = 0; i < _source.Length; i++)
        {
            if (_source[i] % 3 == 0)
            {
                count++;
            }
        }

        Validate(LibraryCatalog.HandCoded, count);
        return count;
    }
}
