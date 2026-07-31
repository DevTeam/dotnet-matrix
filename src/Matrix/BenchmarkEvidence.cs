namespace Matrix;

/// <summary>
/// Provenance for one set of benchmark values. A report can reference several
/// evidence sets after partial runs have been merged.
/// </summary>
public sealed record BenchmarkEvidence(
    string Id,
    string Kind,
    DateTimeOffset GeneratedAtUtc,
    string? CommitSha,
    string? WorkflowRunUrl,
    string Job,
    string EnvironmentId,
    string? ArchiveName,
    string? ManifestPath);
