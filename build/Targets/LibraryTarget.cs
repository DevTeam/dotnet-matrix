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

internal sealed class LibraryTarget(
    IMatrixTarget matrixTarget,
    IReadmeTarget readmeTarget) : ILibraryTarget
{
    public async Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        DiscoveredMatrixModule module,
        MatrixLibrary library,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Updating {module.Metadata.Name}: {library.Name} {library.Version}");
        foreach (var mode in new[] { MatrixMode.Validation, MatrixMode.Benchmark })
        {
            var result = await matrixTarget.RunAsync(
                module,
                mode,
                library.Id,
                false,
                cancellationToken);
            if (result != 0)
            {
                return result;
            }
        }

        return await readmeTarget.RunAsync(modules, cancellationToken);
    }
}
