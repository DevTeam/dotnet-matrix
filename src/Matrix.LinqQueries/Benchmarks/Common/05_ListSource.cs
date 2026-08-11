// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ListSource",
    5,
    "List Source",
    "Runs the canonical filter and projection over a List<int>.")]
public partial class ListSource
{
    private readonly List<int> _source = QueryData.NumberList;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int[] result) =>
        QueryChecks.CanonicalPipeline(library, result);
}
