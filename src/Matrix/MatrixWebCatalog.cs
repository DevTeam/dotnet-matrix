namespace Matrix;

public sealed record MatrixWebCatalog(
    int SchemaVersion,
    GitHubRepository Repository,
    IReadOnlyList<MatrixCategory> Categories,
    IReadOnlyList<MatrixVersion> Versions);

public sealed record GitHubRepository(
    string Owner,
    string Name);

public sealed record MatrixCategory(
    string Id,
    string Name,
    string ReportDirectory);

public sealed record MatrixVersion(
    string Version,
    DateTimeOffset DateUtc,
    string Commit);
