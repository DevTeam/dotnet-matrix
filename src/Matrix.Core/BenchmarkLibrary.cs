namespace Matrix;

public sealed record BenchmarkLibrary(
    string Id,
    string Name,
    string? Package,
    string? Version,
    bool Baseline);