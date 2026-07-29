namespace Build.Targets;

internal sealed record RunConfiguration(
    string Name,
    string Arguments,
    string? FolderName);
