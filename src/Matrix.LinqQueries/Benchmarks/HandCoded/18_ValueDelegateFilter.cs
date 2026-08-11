// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ValueDelegateFilter
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int HandCoded()
    {
        var result = CountWhere(_source, new DivisibleByThree());
        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }

    private static int CountWhere<TPredicate>(int[] source, TPredicate predicate)
        where TPredicate : struct, INumberPredicate
    {
        var count = 0;
        for (var i = 0; i < source.Length; i++)
        {
            if (predicate.Match(source[i]))
            {
                count++;
            }
        }

        return count;
    }
}
