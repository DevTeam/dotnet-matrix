// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

public sealed record FeatureReport(
    int SchemaVersion,
    string? ModuleId,
    DateTimeOffset GeneratedAtUtc,
    string Framework,
    string OperatingSystem,
    IReadOnlyList<MatrixLibrary> Libraries,
    IReadOnlyList<FeatureReportEntry> Features);

public sealed record FeatureReportEntry(
    int Order,
    string Id,
    string Name,
    IReadOnlyList<FeatureResult> Results);

public sealed record FeatureResult(
    string LibraryId,
    string Status,
    string? Reason,
    double DurationMilliseconds);

public sealed record BenchmarkReport(
    int SchemaVersion,
    string? ModuleId,
    DateTimeOffset GeneratedAtUtc,
    IReadOnlyList<BenchmarkEnvironment>? Environments,
    IReadOnlyList<BenchmarkLibrary> Libraries,
    IReadOnlyList<BenchmarkReportEntry> Features);

public sealed record BenchmarkLibrary(
    string Id,
    string Name,
    string? Package,
    string? Version,
    bool Baseline);

public sealed record BenchmarkReportEntry(
    int Order,
    string Id,
    string Name,
    IReadOnlyList<BenchmarkResult> Results);

public sealed record BenchmarkResult(
    string LibraryId,
    bool Successful,
    double? MeanNanoseconds,
    double? StandardErrorNanoseconds,
    double? AllocatedBytesPerOperation,
    string? EnvironmentId);
