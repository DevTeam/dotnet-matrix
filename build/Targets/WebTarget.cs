using HostApi;
using Matrix;
using System.Text.Json;
using System.Text.RegularExpressions;
using HostCommandLine = HostApi.CommandLine;
// ReSharper disable UseCollectionExpression

namespace Build.Targets;

internal sealed partial class WebTarget(
    ICommandLineRunner commandLineRunner,
    IBuildPaths buildPaths,
    IMetadataTarget metadataTarget,
    IReportChartsTarget reportChartsTarget,
    IWebManifestTarget webManifestTarget,
    IQuietProcessRunner processRunner) : IWebTarget
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

        var manifestResult = webManifestTarget.Run(modules);
        if (manifestResult != 0)
        {
            return manifestResult;
        }

        var repository = await ReadRepositoryAsync(cancellationToken);
        var versions = await ReadVersionsAsync(cancellationToken);
        Info($"Releases baked into the catalog: {versions.Count}");
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
            versions);

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
        var commandLine = new HostCommandLine(
            "dotnet",
            buildPaths.SolutionDirectory,
            [
                "publish",
                projectPath,
                "--configuration",
                "Release",
                "--output",
                outputDirectory,
                $"-p:MatrixCatalogPath={catalogPath}",
                "-p:MatrixProduction=true"
            ],
            [],
            "production Web application");

        Info($"Building .NET Matrix for {repository.Owner}/{repository.Name}");
        var result = await processRunner.RunAsync(
            commandLine,
            "production Web application",
            cancellationToken);
        if (result != 0)
        {
            return result;
        }

        var wwwRoot = Path.Combine(outputDirectory, "wwwroot");
        await File.WriteAllTextAsync(Path.Combine(wwwRoot, ".nojekyll"), string.Empty, cancellationToken);
        await WriteNotFoundAsync(wwwRoot, cancellationToken);
        CopyCustomDomain(wwwRoot);
        Info($"Web application: {wwwRoot}");
        return 0;
    }

    /// <summary>
    /// The application has a single route, so an unknown path is always a wrong URL.
    /// Serving a copy of index.html there does not work: its base href is relative,
    /// so the framework files would be requested under the unknown path and 404,
    /// leaving the page stuck on the loading screen. Redirect to the site root
    /// instead, which differs between a project page and a custom domain.
    /// </summary>
    private static async Task WriteNotFoundAsync(
        string wwwRoot,
        CancellationToken cancellationToken) =>
        await File.WriteAllTextAsync(
            Path.Combine(wwwRoot, "404.html"),
            """
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="utf-8"/>
                <title>.NET Matrix</title>
                <script>
                    var root = location.hostname.endsWith('.github.io')
                        ? '/' + location.pathname.split('/')[1] + '/'
                        : '/';
                    if (location.pathname !== root) {
                        location.replace(root);
                    }
                </script>
            </head>
            <body></body>
            </html>

            """,
            cancellationToken);

    /// <summary>
    /// An Actions based deployment publishes exactly the artifact, so the custom
    /// domain has to travel with it or GitHub Pages drops it on the next deploy.
    /// </summary>
    private void CopyCustomDomain(string wwwRoot)
    {
        var source = Path.Combine(buildPaths.SolutionDirectory, "CNAME");
        if (!File.Exists(source))
        {
            return;
        }

        File.Copy(source, Path.Combine(wwwRoot, "CNAME"), true);
        Info($"Custom domain: {File.ReadAllText(source).Trim()}");
    }

    /// <summary>
    /// Releases are baked into the catalog from the local clone, so the published
    /// application needs no GitHub API call to list them. The unauthenticated API
    /// allows 60 requests an hour per address, which a shared network exhausts fast.
    /// </summary>
    private async Task<IReadOnlyList<MatrixVersion>> ReadVersionsAsync(
        CancellationToken cancellationToken)
    {
        var output = await GitAsync(
            [
                "for-each-ref",
                "--format=%(refname:short)\t%(objectname)\t%(*objectname)\t%(creatordate:iso-strict)",
                "refs/tags"
            ],
            cancellationToken);
        var versions = new List<(MatrixVersion Value, Version SortKey)>();
        foreach (var line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = line.Trim().Split('\t');
            if (parts.Length < 4 || !VersionRegex().IsMatch(parts[0]))
            {
                continue;
            }

            // An annotated tag points at a tag object; the dereferenced commit is
            // in the third column and is what the report URLs must use.
            var commit = parts[2].Length > 0 ? parts[2] : parts[1];
            if (!DateTimeOffset.TryParse(parts[3], out var date))
            {
                continue;
            }

            versions.Add((
                new MatrixVersion(parts[0], date, commit, true),
                Version.Parse(parts[0])));
        }

        return versions
            .OrderByDescending(version => version.SortKey)
            .Select(version => version.Value)
            .ToArray();
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
        var standardOutput = new List<string>();
        var standardError = new List<string>();
        var sync = new object();
        ICommandLineResult result;
        var consoleOutput = Console.Out;
        try
        {
            Console.SetOut(TextWriter.Null);
            result = await commandLineRunner.RunAsync(
                new HostCommandLine(
                    "git",
                    buildPaths.SolutionDirectory,
                    arguments,
                    [],
                    $"git {arguments[0]}"),
                output =>
                {
                    output.Handled = true;
                    lock (sync)
                    {
                        (output.IsError ? standardError : standardOutput).Add(output.Line);
                    }
                },
                cancellationToken);
        }
        finally
        {
            Console.SetOut(consoleOutput);
        }

        if (result.State != ProcessState.Finished || result.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"git {string.Join(' ', arguments)} failed: "
                + string.Join(Environment.NewLine, standardError));
        }

        return string.Join(Environment.NewLine, standardOutput);
    }

    [GeneratedRegex(@"github\.com[/:](?<owner>[^/]+)/(?<name>[^/]+?)(?:\.git)?$", RegexOptions.IgnoreCase, "ru-RU")]
    private static partial Regex GitHubRegex();

    [GeneratedRegex(@"^\d+\.\d+\.\d+$", RegexOptions.CultureInvariant)]
    private static partial Regex VersionRegex();
}
