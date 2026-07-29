namespace Matrix;

internal sealed record CapturedFeatureResult(
    int Order,
    string Id,
    string Name,
    FeatureResult Result);
