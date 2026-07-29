namespace Matrix;

public sealed record FeatureReport(
    int SchemaVersion,
    string? ModuleId,
    DateTimeOffset GeneratedAtUtc,
    string Framework,
    string OperatingSystem,
    IReadOnlyList<MatrixLibrary> Libraries,
    IReadOnlyList<FeatureReportEntry> Features);