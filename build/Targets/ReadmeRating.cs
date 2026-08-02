namespace Build.Targets;

public sealed record ReadmeRating(
    int Place,
    string Id,
    string Name,
    int Points,
    int Maximum,
    int Covered,
    int Scenarios,
    string Awards);
