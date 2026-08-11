// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "DistinctValues",
    12,
    "Distinct Values",
    "Reduces 10,000 integers containing 1,000 distinct values to the distinct set.")]
public partial class DistinctValues
{
    private readonly int[] _source = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int[] result) =>
        QueryChecks.DistinctValues(library, result);
}
