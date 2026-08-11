using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class OrderedTopN
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int[] StructLinq()
    {
        var result = _source.ToStructEnumerable().OrderByDescending(static o => o.Amount).Take(20).Select(static o => o.Id, static x => x).ToArray();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
