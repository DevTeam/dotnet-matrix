// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class PagedSlice
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        var result = new int[1000];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = _source[i + 4000];
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
