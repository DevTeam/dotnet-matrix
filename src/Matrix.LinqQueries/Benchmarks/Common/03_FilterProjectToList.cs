// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "FilterProjectToList",
    3,
    "Filter, Project, Materialize",
    "Filters 5,000 orders by amount, projects one property, and materializes a list.")]
public partial class FilterProjectToList
{
    private readonly Order[] _source = QueryData.Orders;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, List<int> result) =>
        QueryChecks.FilterProjectToList(library, result);
}
