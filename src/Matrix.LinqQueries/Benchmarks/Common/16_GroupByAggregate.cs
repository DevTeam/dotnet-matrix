// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "GroupByAggregate",
    16,
    "Group and Aggregate",
    "Groups 5,000 orders by region and produces one summed total per region.")]
public partial class GroupByAggregate
{
    private readonly Order[] _source = QueryData.Orders;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, RegionTotal[] result) =>
        QueryChecks.GroupByAggregate(library, result);
}
