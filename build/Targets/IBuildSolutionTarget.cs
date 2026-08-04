namespace Build.Targets;

internal interface IBuildSolutionTarget
{
    Task<int> RunAsync(CancellationToken cancellationToken);
}
