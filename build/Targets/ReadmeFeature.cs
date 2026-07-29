namespace Build.Targets;

public sealed record ReadmeFeature(
    int Order,
    string Name,
    string? Description,
    string ChartPath);
