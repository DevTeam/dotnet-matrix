namespace Build.Targets;

public sealed record ReadmeLibrary(
    string Name,
    string Version,
    string Description,
    string? Url,
    string Logo);
