// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ValueDelegateFilter",
    18,
    "Struct Predicate Filter",
    "Filters and counts 10,000 integers with a struct-typed predicate instead of a delegate.")]
public partial class ValueDelegateFilter
{
    private readonly int[] _source = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int result) =>
        QueryChecks.ValueDelegateFilter(library, result);
}
