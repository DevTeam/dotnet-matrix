// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ZipPairs
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        var length = Math.Min(_first.Length, _second.Length);
        var result = new int[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = _first[i] * _second[i];
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
