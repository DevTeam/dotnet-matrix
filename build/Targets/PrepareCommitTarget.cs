using Matrix;

namespace Build.Targets;

internal sealed class PrepareCommitTarget(
    IMatrixTarget matrixTarget,
    IRunConfigurationsTarget runConfigurationsTarget,
    IWebManifestTarget webManifestTarget,
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
            Host.Info("Using the reports already present in 'reports'.");
        }

        var result = runConfigurationsTarget.Run(modules);
        if (result != 0)
        {
            return result;
        }

        result = webManifestTarget.Run(modules);
        if (result != 0)
        {
            return result;
        }

        result = await readmeTarget.RunAsync(modules, cancellationToken);
        if (result == 0)
        {
            Host.Info("Source-controlled report artifacts are ready.");
        }

        return result;
    }
}
