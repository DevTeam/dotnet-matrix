// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "FilterCount",
    1,
    "Filter and Count",
    "Filters 10,000 integers with a predicate and counts the survivors.")]
public partial class FilterCount
{
    private readonly int[] _source = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int result) =>
        QueryChecks.FilterCount(library, result);
}
