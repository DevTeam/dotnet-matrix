// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "FirstMatch",
    10,
    "First Match",
    "Returns the first of 5,000 orders satisfying a predicate first met at index 4,500.")]
public partial class FirstMatch
{
    private readonly Order[] _source = QueryData.Orders;

    [Conditional("MATRIX_VALIDATION")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void Validate(string library, Order result) =>
        QueryChecks.FirstMatch(library, result);
}
