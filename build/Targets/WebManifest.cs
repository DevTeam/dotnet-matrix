namespace Build.Targets;

/// <summary>
/// The installation manifest Chrome reads. Property names follow the web application
/// manifest specification, which is snake case; the serializer applies the policy.
/// </summary>
internal sealed record WebManifest(
    string Id,
    string Name,
    string ShortName,
    string Description,
    string StartUrl,
    string Scope,
    string Display,
    string BackgroundColor,
    string ThemeColor,
    bool PreferRelatedApplications,
    IReadOnlyList<WebManifestIcon> Icons,
    IReadOnlyList<WebManifestShortcut> Shortcuts);

internal sealed record WebManifestIcon(
    string Src,
    string Type,
    string Sizes,
    string? Purpose);

internal sealed record WebManifestShortcut(
    string Name,
    string ShortName,
    string Url);
