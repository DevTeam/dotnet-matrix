// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

public sealed record MatrixLibraryMetadataCatalog(
    int SchemaVersion,
    IReadOnlyList<MatrixLibraryMetadata> Libraries);