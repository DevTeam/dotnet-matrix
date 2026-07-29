namespace Build.Targets;

public sealed record ReadmeModel(
    string ApplicationUrl,
    IReadOnlyList<ReadmeCategory> Categories);
