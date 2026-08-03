namespace Build.Targets;

public sealed record ReadmeRating(
    int Place,
    string Id,
    string Name,
    string Points,
    string TimePoints,
    string MemoryPoints,
    int Maximum,
    int MetricMaximum,
    int Covered,
    int Scenarios,
    string Awards,
    IReadOnlyList<ReadmeScore> Breakdown);
