// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "JoinLookup",
    17,
    "Join and Project",
    "Joins 5,000 orders to 500 customers on the customer key and projects the pairs.")]
public partial class JoinLookup
{
    private readonly Order[] _orders = QueryData.Orders;
    private readonly Customer[] _customers = QueryData.Customers;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, CustomerOrder[] result) =>
        QueryChecks.JoinLookup(library, result);
}
