// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "FirstMatch",
    10,
    "First Match",
    "Returns the first of 5,000 orders satisfying a predicate first met at index 4,500.")]
public partial class FirstMatch
{
    private readonly Order[] _source = QueryData.Orders;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, Order result) =>
        QueryChecks.FirstMatch(library, result);
}
