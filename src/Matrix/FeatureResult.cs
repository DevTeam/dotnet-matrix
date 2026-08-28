// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix;

public sealed record FeatureResult(
    string LibraryId,
    string Status,
    string? Reason,
    double DurationMilliseconds,
    string? Note = null);
