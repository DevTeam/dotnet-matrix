// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "OpaqueSource",
    6,
    "Opaque Source",
    "Runs the same query over an IEnumerable<int> exposing no count, indexer, or array.")]
public partial class OpaqueSource
{
    private readonly Func<IEnumerable<int>> _source = QueryData.Opaque;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int[] result) =>
        QueryChecks.CanonicalPipeline(library, result);
}
