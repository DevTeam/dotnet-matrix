using ZLinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class JoinLookup
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLinq)]
    public CustomerOrder[] ZLinq()
    {
        var result = _orders.AsValueEnumerable().Join(_customers.AsValueEnumerable(), static o => o.CustomerId, static c => c.Id, static (o, c) => new CustomerOrder(o.Id, c.Name)).ToArray();
        Validate(LibraryCatalog.ZLinq, result);
        return result;
    }
}
