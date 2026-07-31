namespace Build.Targets;

public sealed record ReadmeRating(
    int Place,
    string Id,
    string Name,
    int Gold,
    int Silver,
    int Bronze,
    string Awards);
