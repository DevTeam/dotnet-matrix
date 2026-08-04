// ReSharper disable UseCollectionExpression
namespace Matrix;

public static class BenchmarkEnvironmentComparer
{
    public static IReadOnlyList<BenchmarkEnvironmentDifference> GetDifferences(
        BenchmarkEnvironment existing,
        BenchmarkEnvironment current) =>
        typeof(BenchmarkEnvironment)
            .GetProperties()
            .Where(property => property.Name != nameof(BenchmarkEnvironment.Id))
            .Select(property => new BenchmarkEnvironmentDifference(
                property.Name,
                property.GetValue(existing)?.ToString() ?? string.Empty,
                property.GetValue(current)?.ToString() ?? string.Empty))
            .Where(difference => difference.Existing != difference.Current)
            .ToArray();
}