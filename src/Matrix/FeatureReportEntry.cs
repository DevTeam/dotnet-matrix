namespace Matrix;

public sealed record FeatureReportEntry(
    int Order,
    string Id,
    string Name,
    IReadOnlyList<FeatureResult> Results);