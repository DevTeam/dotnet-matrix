using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterCount
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int StructLinq()
    {
        var result = _source.ToStructEnumerable().Where(static n => n % 3 == 0).Count();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
