using System.Linq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class JoinLookup
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemLinq)]
    public CustomerOrder[] SystemLinq()
    {
        var result = _orders.Join(_customers, static o => o.CustomerId, static c => c.Id, static (o, c) => new CustomerOrder(o.Id, c.Name)).ToArray();
        Validate(LibraryCatalog.SystemLinq, result);
        return result;
    }
}
