using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class AnyMatch
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public bool Hyperlinq()
    {
        var result = _source.AsValueEnumerable().Any(static n => n >= 700);
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
