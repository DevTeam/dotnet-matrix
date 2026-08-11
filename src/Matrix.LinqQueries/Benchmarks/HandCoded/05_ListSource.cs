// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ListSource
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        var values = new List<int>();
        for (var i = 0; i < _source.Count; i++)
        {
            var value = _source[i];
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
