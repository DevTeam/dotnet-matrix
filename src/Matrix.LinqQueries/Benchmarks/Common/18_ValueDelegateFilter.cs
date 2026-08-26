// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ValueDelegateFilter",
    18,
    "Struct Predicate Filter",
    "Filters and counts 10,000 integers with a struct-typed predicate instead of a delegate.",
    rated: false,
    reason: "With this few rated entrants, the reference is a library's own result, not a result earned against a competitor, so the full 200 points would not reflect a win.")]
public partial class ValueDelegateFilter
{
    private readonly int[] _source = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int result) =>
        QueryChecks.ValueDelegateFilter(library, result);
}
