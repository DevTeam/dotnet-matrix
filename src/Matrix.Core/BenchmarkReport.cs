// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

public sealed record BenchmarkReport(
    int SchemaVersion,
    string? ModuleId,
    DateTimeOffset GeneratedAtUtc,
    IReadOnlyList<BenchmarkEnvironment>? Environments,
    IReadOnlyList<BenchmarkLibrary> Libraries,
    IReadOnlyList<BenchmarkReportEntry> Features,
    IReadOnlyList<BenchmarkEvidence>? Evidence = null);
