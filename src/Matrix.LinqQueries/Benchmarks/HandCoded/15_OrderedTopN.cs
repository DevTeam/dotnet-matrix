// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class OrderedTopN
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        var ordered = (Order[])_source.Clone();
        Array.Sort(ordered, static (left, right) => right.Amount.CompareTo(left.Amount));
        var result = new int[20];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = ordered[i].Id;
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
