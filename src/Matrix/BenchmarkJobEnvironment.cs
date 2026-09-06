namespace Matrix;

public sealed record BenchmarkJobEnvironment(
    string? Framework,
    string? ProcessArchitecture,
    bool? ServerGarbageCollector);
