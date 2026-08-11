using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FirstMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public Order StructLinq()
    {
        var result = _source.ToStructEnumerable().First(static o => o.Amount >= 10_000);
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
