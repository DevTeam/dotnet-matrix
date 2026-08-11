using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterCount
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public int Hyperlinq()
    {
        var result = _source.AsValueEnumerable().Where(static n => n % 3 == 0).Count();
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
