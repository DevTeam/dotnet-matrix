using System.Text.Json;
using Matrix;
// ReSharper disable InvertIf

namespace Build.Targets;

internal sealed class MetadataTarget(
    IBuildPaths buildPaths,
    IJsonSerializer jsonSerializer) : IMetadataTarget
{
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };

    public int Run(IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        foreach (var module in modules)
        {
            var directory = Path.Combine(
                buildPaths.SolutionDirectory,
                "metadata",
                module.Metadata.ReportDirectory);
            Directory.CreateDirectory(directory);
            foreach (var library in module.Metadata.LibraryMetadata.Libraries)
            {
                var logoPath = Path.Combine(directory, library.Logo);
                if (!File.Exists(logoPath))
                {
                    Console.Error.WriteLine(
                        $"Metadata logo for '{library.Id}' does not exist: {logoPath}");
                    return 1;
                }
            }

            var path = Path.Combine(directory, "libraries.json");
            File.WriteAllText(
                path,
                jsonSerializer.Serialize(module.Metadata.LibraryMetadata, JsonOptions)
                + Environment.NewLine);
            var featurePath = Path.Combine(directory, "features.json");
            File.WriteAllText(
                featurePath,
                jsonSerializer.Serialize(module.Metadata.FeatureMetadata, JsonOptions)
                + Environment.NewLine);
        }

        Info($"Metadata: {modules.Count} categories generated.");
        return 0;
    }
}
