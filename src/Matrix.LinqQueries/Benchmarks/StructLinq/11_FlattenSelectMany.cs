using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FlattenSelectMany
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int[] StructLinq()
    {
        var result = _source.ToStructEnumerable().SelectMany(static batch => (IEnumerable<int>)batch).ToArray();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
