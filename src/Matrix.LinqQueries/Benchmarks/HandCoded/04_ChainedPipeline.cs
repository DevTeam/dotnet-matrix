// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ChainedPipeline
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        var values = new int[1000];
        var count = 0;
        for (var i = 0; i < _source.Length && count < values.Length; i++)
        {
            var value = _source[i];
            if (value % 3 != 0)
            {
                continue;
            }

            var projection = value * 2;
            if (projection % 4 == 0)
            {
                values[count++] = projection;
            }
        }

        if (count != values.Length)
        {
            Array.Resize(ref values, count);
        }

        Validate(LibraryCatalog.HandCoded, values);
        return values;
    }
}
