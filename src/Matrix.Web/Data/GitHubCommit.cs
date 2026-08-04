// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global
namespace Matrix.Web;

internal sealed record GitHubCommit(
    string Sha,
    GitHubCommitData Commit);
