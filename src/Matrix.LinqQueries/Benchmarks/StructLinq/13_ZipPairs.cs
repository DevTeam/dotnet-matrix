using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ZipPairs
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int[] StructLinq()
    {
        var result = _first.ToStructEnumerable().Zip(_second.ToStructEnumerable()).Select(static pair => pair.Item1 * pair.Item2, static x => x).ToArray();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
