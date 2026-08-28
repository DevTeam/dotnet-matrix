using Matrix;

namespace Build.Targets;

internal sealed class PrepareCommitTarget(
    IMatrixTarget matrixTarget,
    IAotProbesTarget aotProbesTarget,
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
            Info("Using the reports already present in 'reports'.");
        }

        // Feature validation rewrites features.json from the scenarios it discovers, which drops
        // any entry it did not produce. The Native AOT feature therefore has to be probed again
        // after every validation pass, and before the artifacts below read the reports.
        if (runMatrix)
        {
            var probes = await aotProbesTarget.RunAsync(modules, cancellationToken);
            if (probes != 0)
            {
                return probes;
            }
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
            Info("Source-controlled report artifacts are ready.");
        }

        return result;
    }
}
