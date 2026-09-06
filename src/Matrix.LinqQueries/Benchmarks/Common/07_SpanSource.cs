// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SpanSource",
    7,
    "Span Source",
    "Runs the same query over a ReadOnlySpan<int>.")]
public partial class SpanSource
{
    private readonly int[] _source = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void Validate(string library, int[] result) =>
        QueryChecks.CanonicalPipeline(library, result);
}
