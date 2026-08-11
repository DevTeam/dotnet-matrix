using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class AnyMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public bool StructLinq()
    {
        var result = _source.ToStructEnumerable().Any(static n => n >= 700);
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
