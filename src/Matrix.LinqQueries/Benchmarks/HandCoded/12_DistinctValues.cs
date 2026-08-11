// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class DistinctValues
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        var seen = new HashSet<int>();
        var values = new List<int>();
        for (var i = 0; i < _source.Length; i++)
        {
            if (seen.Add(_source[i]))
            {
                values.Add(_source[i]);
            }
        }

        var result = values.ToArray();
        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
