// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ChainedPipeline",
    4,
    "Chained Pipeline",
    "Composes a filter, a projection, a second filter, and a limit into one four-stage query.")]
public partial class ChainedPipeline
{
    private readonly int[] _source = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int[] result) =>
        QueryChecks.ChainedPipeline(library, result);
}
