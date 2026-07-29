namespace Matrix;

internal sealed record CapturedBenchmarkResult(
    int Order,
    string Id,
    string Name,
    BenchmarkResult Result);
