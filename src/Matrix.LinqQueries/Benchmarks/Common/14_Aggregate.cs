// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Aggregate",
    14,
    "Aggregate",
    "Folds 10,000 integers with a caller-supplied accumulator and a seed.")]
public partial class Aggregate
{
    private readonly int[] _source = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int result) =>
        QueryChecks.Aggregate(library, result);
}
