// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace Matrix.LinqQueries.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ProjectToArray",
    2,
    "Project To Array",
    "Projects 10,000 integers through a selector and materializes a new array.")]
public partial class ProjectToArray
{
    private readonly int[] _source = QueryData.Numbers;

    [Conditional("MATRIX_VALIDATION")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void Validate(string library, int[] result) =>
        QueryChecks.ProjectToArray(library, result);
}
