namespace Build.Targets;

public sealed record ReadmeRating(
    int Place,
    string Name,
    int Gold,
    int Silver,
    int Bronze,
    string Awards);
