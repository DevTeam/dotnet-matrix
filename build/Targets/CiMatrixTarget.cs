using System.Text.Json;

namespace Build.Targets;

internal sealed class CiMatrixTarget(IBuildPaths buildPaths) : ICiMatrixTarget
{
    public int Run(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        string? outputFile)
    {
        var path = Path.GetFullPath(
            outputFile ?? Path.Combine("artifacts", "ci-matrix.json"),
            buildPaths.SolutionDirectory);
        var directory = Path.GetDirectoryName(path);
        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        var categories = modules.Select(module => module.Metadata.Id).ToArray();
        File.WriteAllText(path, JsonSerializer.Serialize(categories));
        Console.WriteLine($"CI category matrix: {path}");
        return 0;
    }
}
