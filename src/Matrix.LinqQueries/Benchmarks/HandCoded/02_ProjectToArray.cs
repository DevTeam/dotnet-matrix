// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ProjectToArray
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        var result = new int[_source.Length];
        for (var i = 0; i < _source.Length; i++)
        {
            result[i] = _source[i] * 2;
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
