namespace Matrix;

public sealed record FeatureResult(
    string LibraryId,
    string Status,
    string? Reason,
    double DurationMilliseconds);