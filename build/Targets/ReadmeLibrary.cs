namespace Build.Targets;

public sealed record ReadmeLibrary(
    string Id,
    string Name,
    string Version,
    string Description,
    string? Url,
    string Logo);
