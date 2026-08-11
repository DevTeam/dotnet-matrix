using LinqAF;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class JoinLookup
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LinqAF)]
    public CustomerOrder[] LinqAF()
    {
        var result = _orders.Join(_customers, static o => o.CustomerId, static c => c.Id, static (o, c) => new CustomerOrder(o.Id, c.Name)).ToArray();
        Validate(LibraryCatalog.LinqAF, result);
        return result;
    }
}
