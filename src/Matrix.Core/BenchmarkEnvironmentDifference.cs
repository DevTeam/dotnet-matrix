namespace Matrix;

public sealed record BenchmarkEnvironmentDifference(
    string Name,
    string Existing,
    string Current);