namespace Matrix;

/// <summary>
/// <paramref name="DateUtc"/> is null until it is fetched, which happens only for
/// the version actually opened: dating every tag costs one GitHub API call each and
/// exhausts the unauthenticated rate limit on repositories with many tags.
/// </summary>
/// <remarks>
/// <paramref name="Commit"/> is what the report URLs point at: a commit for a
/// release, empty for the local workspace.
/// <paramref name="Released"/> separates tags, whose numbers are fixed forever,
/// from the workspace, which can change under the reader.
/// </remarks>
public sealed record MatrixVersion(
    string Version,
    DateTimeOffset? DateUtc,
    string Commit,
    bool Released);