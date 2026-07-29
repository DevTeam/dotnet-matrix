namespace Matrix;

public sealed record BenchmarkReportEntry(
    int Order,
    string Id,
    string Name,
    IReadOnlyList<BenchmarkResult> Results);