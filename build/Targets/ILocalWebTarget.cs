namespace Build.Targets;

internal interface ILocalWebTarget
{
    Task<int> RunAsync(bool launchBrowser, CancellationToken cancellationToken);
}
