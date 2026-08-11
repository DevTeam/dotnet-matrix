// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class JoinLookup
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public CustomerOrder[] HandCoded()
    {
        var customers = new Dictionary<int, string>(_customers.Length);
        for (var i = 0; i < _customers.Length; i++)
        {
            customers.Add(_customers[i].Id, _customers[i].Name);
        }

        var result = new CustomerOrder[_orders.Length];
        for (var i = 0; i < _orders.Length; i++)
        {
            var order = _orders[i];
            result[i] = new CustomerOrder(order.Id, customers[order.CustomerId]);
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
