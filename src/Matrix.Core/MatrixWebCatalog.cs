namespace Matrix;

public sealed record MatrixWebCatalog(
    int SchemaVersion,
    GitHubRepository Repository,
    IReadOnlyList<MatrixCategory> Categories,
    IReadOnlyList<MatrixVersion> Versions);