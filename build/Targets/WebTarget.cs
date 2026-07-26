using Matrix;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
// ReSharper disable UseCollectionExpression

namespace Build.Targets;

internal interface IWebTarget
{
    Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CancellationToken cancellationToken);
}

internal sealed partial class WebTarget(
    IBuildPaths buildPaths,
    IMetadataTarget metadataTarget,
    IReportChartsTarget reportChartsTarget) : IWebTarget
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public async Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CancellationToken cancellationToken)
    {
        var metadataResult = metadataTarget.Run(modules);
        if (metadataResult != 0)
        {
            return metadataResult;
        }

        var chartsResult = reportChartsTarget.Run(modules);
        if (chartsResult != 0)
        {
            return chartsResult;
        }

        var repository = await ReadRepositoryAsync(cancellationToken);
        var catalog = new MatrixWebCatalog(
            1,
            repository,
            modules
                .Select(module => new MatrixCategory(
                    module.Metadata.Id,
                    module.Metadata.Name,
                    module.Metadata.ReportDirectory))
                .OrderBy(category => category.Name, StringComparer.OrdinalIgnoreCase)
                .ToArray(),
            []);

        var artifactsDirectory = Path.Combine(buildPaths.SolutionDirectory, "artifacts");
        var generatedDirectory = Path.Combine(artifactsDirectory, "generated");
        var outputDirectory = Path.Combine(artifactsDirectory, "web");
        Directory.CreateDirectory(generatedDirectory);
        var catalogPath = Path.Combine(generatedDirectory, "catalog.json");
        await File.WriteAllTextAsync(
            catalogPath,
            JsonSerializer.Serialize(catalog, SerializerOptions),
            cancellationToken);

        var projectPath = Path.Combine(
            buildPaths.SolutionDirectory,
            "src",
            "Matrix.Web",
            "Matrix.Web.csproj");
        var startInfo = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = buildPaths.SolutionDirectory,
            UseShellExecute = false
        };
        startInfo.ArgumentList.Add("publish");
        startInfo.ArgumentList.Add(projectPath);
        startInfo.ArgumentList.Add("--configuration");
        startInfo.ArgumentList.Add("Release");
        startInfo.ArgumentList.Add("--output");
        startInfo.ArgumentList.Add(outputDirectory);
        startInfo.ArgumentList.Add($"-p:MatrixCatalogPath={catalogPath}");
        startInfo.ArgumentList.Add("-p:MatrixProduction=true");

        Console.WriteLine($"Building .NET Matrix for {repository.Owner}/{repository.Name}");
        using var process = Process.Start(startInfo)
                            ?? throw new InvalidOperationException("Could not start dotnet publish.");
        await process.WaitForExitAsync(cancellationToken);
        if (process.ExitCode != 0)
        {
            return process.ExitCode;
        }

        var wwwRoot = Path.Combine(outputDirectory, "wwwroot");
        await File.WriteAllTextAsync(Path.Combine(wwwRoot, ".nojekyll"), string.Empty, cancellationToken);
        File.Copy(
            Path.Combine(wwwRoot, "index.html"),
            Path.Combine(wwwRoot, "404.html"),
            true);
        Console.WriteLine($"Web application: {wwwRoot}");
        return 0;
    }

    private async Task<GitHubRepository> ReadRepositoryAsync(CancellationToken cancellationToken)
    {
        var remote = (await GitAsync(["remote", "get-url", "origin"], cancellationToken)).Trim();
        var match = GitHubRegex().Match(remote);
        if (!match.Success)
        {
            throw new InvalidOperationException(
                $"Cannot determine GitHub repository from origin remote '{remote}'.");
        }

        return new GitHubRepository(
            match.Groups["owner"].Value,
            match.Groups["name"].Value);
    }

    private async Task<string> GitAsync(
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo("git")
        {
            WorkingDirectory = buildPaths.SolutionDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)
                            ?? throw new InvalidOperationException("Could not start git.");
        var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);
        var output = await outputTask;
        var error = await errorTask;
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"git {string.Join(' ', arguments)} failed: {error.Trim()}");
        }

        return output;
    }

    [GeneratedRegex(@"github\.com[/:](?<owner>[^/]+)/(?<name>[^/]+?)(?:\.git)?$", RegexOptions.IgnoreCase, "ru-RU")]
    private static partial Regex GitHubRegex();
}
