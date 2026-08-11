// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "FlattenSelectMany",
    11,
    "Flatten Nested Sequences",
    "Flattens 500 inner arrays of 20 integers and materializes the result.")]
public partial class FlattenSelectMany
{
    private readonly int[][] _source = QueryData.Batches;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int[] result) =>
        QueryChecks.FlattenSelectMany(library, result);
}
