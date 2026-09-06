// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ZipPairs",
    13,
    "Zip Pairs",
    "Pairs two sequences of 10,000 integers and materializes the products.")]
public partial class ZipPairs
{
    private readonly int[] _first = QueryData.Numbers;
    private readonly int[] _second = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void Validate(string library, int[] result) =>
        QueryChecks.ZipPairs(library, result);
}
