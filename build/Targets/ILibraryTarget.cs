using Matrix;

namespace Build.Targets;

internal interface ILibraryTarget
{
    Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        DiscoveredMatrixModule module,
        MatrixLibrary library,
        CancellationToken cancellationToken);
}
