using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class ValueDelegateFilter
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public int StructLinq()
    {
        var predicate = new DivisibleByThree();
        var result = _source.ToStructEnumerable().Where(ref predicate, static x => x).Count();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
