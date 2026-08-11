using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ChainedPipeline
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int[] StructLinq()
    {
        var result = _source.ToStructEnumerable().Where(static n => n % 3 == 0).Select(static n => n * 2, static x => x).Where(static n => n % 4 == 0).Take(1000).ToArray();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
