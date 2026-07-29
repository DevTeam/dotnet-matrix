namespace Matrix.Web;

internal sealed record GitHubCommit(
    string Sha,
    GitHubCommitData Commit);
