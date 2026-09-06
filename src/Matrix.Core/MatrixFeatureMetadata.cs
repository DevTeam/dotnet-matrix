// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

public sealed record MatrixFeatureMetadata(
    string Id,
    int Order,
    string Name,
    string Description,
    bool Rated = true,
    string? Reason = null);