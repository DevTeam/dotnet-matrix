// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "AnyMatch",
    9,
    "Any Match",
    "Reports whether any of 10,000 integers satisfies a predicate first met at index 9,500.")]
public partial class AnyMatch
{
    private readonly int[] _source = QueryData.ScanNumbers;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, bool result) =>
        QueryChecks.AnyMatch(library, result);
}
