namespace Matrix.Web;

internal sealed record GitHubTag(
    string Name,
    GitHubTagCommit Commit);
