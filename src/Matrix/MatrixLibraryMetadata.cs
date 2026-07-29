// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

public sealed record MatrixLibraryMetadata(
    string Id,
    string Description,
    string? DocumentationUrl,
    string? RepositoryUrl,
    string Logo,
    bool Rated);
