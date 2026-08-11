using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class Aggregate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int StructLinq()
    {
        var result = _source.ToStructEnumerable().Aggregate(0, static (sum, value) => sum + value);
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
