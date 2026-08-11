// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "OrderedTopN",
    15,
    "Ordered Top N",
    "Orders 5,000 orders by descending amount and materializes the identifiers of the top 20.")]
public partial class OrderedTopN
{
    private readonly Order[] _source = QueryData.Orders;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int[] result) =>
        QueryChecks.OrderedTopN(library, result);
}
