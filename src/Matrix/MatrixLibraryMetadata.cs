// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

public sealed record MatrixLibraryMetadataCatalog(
    int SchemaVersion,
    IReadOnlyList<MatrixLibraryMetadata> Libraries);

public sealed record MatrixLibraryMetadata(
    string Id,
    string Description,
    string? DocumentationUrl,
    string? RepositoryUrl,
    string Logo);
