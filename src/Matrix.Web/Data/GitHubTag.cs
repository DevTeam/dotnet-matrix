// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
namespace Matrix.Web;

internal sealed record GitHubTag(
    string Name,
    GitHubTagCommit Commit);
