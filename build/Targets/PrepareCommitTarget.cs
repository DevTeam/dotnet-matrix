using Matrix;

namespace Build.Targets;

internal sealed class PrepareCommitTarget(
    IMatrixTarget matrixTarget,
    IRunConfigurationsTarget runConfigurationsTarget,
    IReadmeTarget readmeTarget) : IPrepareCommitTarget
{
    public async Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        bool runMatrix,
        CancellationToken cancellationToken)
    {
        if (runMatrix)
        {
            foreach (var mode in new[] { MatrixMode.Validation, MatrixMode.Benchmark })
            {
                foreach (var module in modules)
                {
                    var matrixResult = await matrixTarget.RunAsync(
                        module,
                        mode,
                        null,
                        false,
                        cancellationToken);
                    if (matrixResult != 0)
                    {
                        return matrixResult;
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Using the reports already present in 'reports'.");
        }

        var result = runConfigurationsTarget.Run(modules);
        return result != 0
            ? result
            : await readmeTarget.RunAsync(modules, cancellationToken);
    }
}
