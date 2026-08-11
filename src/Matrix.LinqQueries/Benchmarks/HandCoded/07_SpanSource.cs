// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class SpanSource
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        ReadOnlySpan<int> source = _source;
        var values = new List<int>();
        for (var i = 0; i < source.Length; i++)
        {
            var value = source[i];
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
