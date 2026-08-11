using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class OpaqueSource
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int[] StructLinq()
    {
        var result = _source().ToStructEnumerable().Where(static n => n % 3 == 0).Select(static n => n * 2, static x => x).ToArray();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
