// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

public sealed record MatrixFeatureCatalog(
    int SchemaVersion,
    IReadOnlyList<MatrixFeatureMetadata> Features);
