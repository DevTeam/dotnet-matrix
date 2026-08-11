// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "PagedSlice",
    8,
    "Paged Slice",
    "Skips 4,000 integers, takes the next 1,000, and materializes them.")]
public partial class PagedSlice
{
    private readonly int[] _source = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, int[] result) =>
        QueryChecks.PagedSlice(library, result);
}
