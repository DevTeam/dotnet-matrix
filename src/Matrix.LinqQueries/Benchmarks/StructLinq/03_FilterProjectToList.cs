using StructLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FilterProjectToList
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.StructLinq)]
    public List<int> StructLinq()
    {
        var result = _source.ToStructEnumerable().Where(static o => o.Amount > 2500).Select(static o => o.Id, static x => x).ToList();
        Validate(LibraryCatalog.StructLinq, result);
        return result;
    }
}
