// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

public sealed record BenchmarkResult(
    string LibraryId,
    bool Successful,
    double? MeanNanoseconds,
    double? StandardErrorNanoseconds,
    double? AllocatedBytesPerOperation,
    string? EnvironmentId,
    int? PayloadSizeBytes = null,
    string? EvidenceId = null);
