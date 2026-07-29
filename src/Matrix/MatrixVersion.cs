namespace Matrix;

/// <summary>
/// <paramref name="DateUtc"/> is null until it is fetched, which happens only for
/// the version actually opened: dating every tag costs one GitHub API call each and
/// exhausts the unauthenticated rate limit on repositories with many tags.
/// </summary>
public sealed record MatrixVersion(
    string Version,
    DateTimeOffset? DateUtc,
    string Commit);