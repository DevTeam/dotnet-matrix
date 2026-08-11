// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class OpaqueSource
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        var values = new List<int>();
        foreach (var value in _source())
        {
            if (value % 3 == 0)
            {
                values.Add(value * 2);
            }
        }

        var result = values.ToArray();
        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
