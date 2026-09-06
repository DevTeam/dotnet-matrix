namespace Matrix;

public sealed record BenchmarkExecution(
    BenchmarkEnvironment Environment,
    IReadOnlyList<CapturedBenchmarkResult> Results);
