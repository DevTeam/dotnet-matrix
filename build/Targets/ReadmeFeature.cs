namespace Build.Targets;

public sealed record ReadmeFeature(
    string Id,
    int Order,
    string Name,
    string? Description,
    string ChartPath,
    string? NotRatedReason);
