// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class AnyMatch
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public bool HandCoded()
    {
        var result = false;
        for (var i = 0; i < _source.Length; i++)
        {
            if (_source[i] >= 700)
            {
                result = true;
                break;
            }
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
