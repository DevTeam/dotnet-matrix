namespace Matrix;

public sealed record CapturedBenchmarkResult(
    int Order,
    string Id,
    string Name,
    BenchmarkResult Result);
