// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

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
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void Validate(string library, int[] result) =>
        QueryChecks.CanonicalPipeline(library, result);
}
