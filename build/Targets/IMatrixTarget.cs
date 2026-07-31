using Matrix;

namespace Build.Targets;

internal interface IMatrixTarget
{
    Task<int> RunAsync(
        DiscoveredMatrixModule module,
        MatrixMode mode,
        string? libraries,
        bool smoke,
        CancellationToken cancellationToken,
        string? evidenceDirectory = null);
}
