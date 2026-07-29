using Matrix;
using System.Text.Json;

namespace Build.Targets;

internal interface IReadmeTarget
{
    Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CancellationToken cancellationToken);
}

internal sealed class ReadmeTarget(
    IBuildPaths buildPaths,
    IMetadataTarget metadataTarget,
    IReportChartsTarget reportChartsTarget,
    ITemplateEngine templateEngine) : IReadmeTarget
{
    private const string Template = "/Templates/Readme.cshtml";
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public async Task<int> RunAsync(
        IReadOnlyList<DiscoveredMatrixModule> modules,
        CancellationToken cancellationToken)
    {
        var metadataResult = metadataTarget.Run(modules);
        if (metadataResult != 0)
        {
            return metadataResult;
        }

        var result = reportChartsTarget.Run(modules);
        if (result != 0)
        {
            return result;
        }

        var categories = modules
            .Select(CreateCategory)
            .Where(category => category is not null)
            .Select(category => category!)
            .ToArray();
        var catalog = Read<MatrixWebCatalog>(Path.Combine(
            buildPaths.SolutionDirectory,
            "src",
            "Matrix.Web",
            "wwwroot",
            "data",
            "catalog.json"));
        var applicationUrl =
            $"https://{catalog.Repository.Owner.ToLowerInvariant()}.github.io/"
            + $"{catalog.Repository.Name}/";
        var model = new ReadmeModel(applicationUrl, categories);
        var path = Path.Combine(buildPaths.SolutionDirectory, "README.md");
        await using var stream = File.Create(path);
        await templateEngine.RenderAsync(
            Template,
            model,
            stream,
            cancellationToken);
        Console.WriteLine($"README: {path}");
        return 0;
    }

    private ReadmeCategory? CreateCategory(DiscoveredMatrixModule module)
    {
        var reportRoot = Path.Combine(
            buildPaths.SolutionDirectory,
            "reports",
            module.Metadata.ReportDirectory);
        var metadataRoot = Path.Combine(
            buildPaths.SolutionDirectory,
            "metadata",
            module.Metadata.ReportDirectory);
        var reportPath = Path.Combine(reportRoot, "benchmarks.json");
        var chartsPath = Path.Combine(metadataRoot, "charts.json");
        var librariesPath = Path.Combine(metadataRoot, "libraries.json");
        if (!File.Exists(reportPath) || !File.Exists(chartsPath) || !File.Exists(librariesPath))
        {
            Console.Error.WriteLine(
                $"WARNING: README data for {module.Metadata.Name} is incomplete.");
            return null;
        }

        var report = Read<BenchmarkReport>(reportPath);
        var charts = Read<MatrixChartCatalog>(chartsPath);
        var metadata = Read<MatrixLibraryMetadataCatalog>(librariesPath);
        var moduleLibraries = module.Metadata.Libraries.ToDictionary(
            library => library.Id,
            StringComparer.OrdinalIgnoreCase);
        var libraries = metadata.Libraries
            .Select(library =>
            {
                moduleLibraries.TryGetValue(library.Id, out var moduleLibrary);
                return new ReadmeLibrary(
                    moduleLibrary?.Name ?? library.Id,
                    moduleLibrary?.Version ?? string.Empty,
                    library.Description,
                    library.DocumentationUrl ?? library.RepositoryUrl,
                    RelativePath(
                        Path.Combine(metadataRoot, library.Logo)));
            })
            .OrderBy(library => library.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var overviews = charts.Groups
            .Select(group => new ReadmeChart(
                group.Name,
                RelativePath(Path.Combine(
                    reportRoot,
                    MatrixChartPaths.DirectoryName,
                    MatrixChartPaths.Overview(group)))))
            .ToArray();
        var features = report.Features
            .OrderBy(feature => feature.Order)
            .Select(feature => new ReadmeFeature(
                feature.Order,
                feature.Name,
                module.Metadata.FeatureMetadata.Features
                    .FirstOrDefault(item =>
                        item.Id.Equals(feature.Id, StringComparison.OrdinalIgnoreCase))
                    ?.Description,
                RelativePath(Path.Combine(
                    reportRoot,
                    MatrixChartPaths.DirectoryName,
                    MatrixChartPaths.Feature(feature)))))
            .ToArray();
        var rated = module.Metadata.LibraryMetadata.Libraries
            .Where(library => library.Rated)
            .Select(library => library.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var rating = MatrixRatings
            .Create(report, charts, libraryId => rated.Contains(libraryId))
            .Select((medals, index) => new ReadmeRating(
                index + 1,
                medals.Name,
                medals.Gold,
                medals.Silver,
                medals.Bronze,
                string.Join(
                    ", ",
                    medals.Awards.Select(award =>
                        $"{Place(award.Place)} in {award.GroupName}"))))
            .ToArray();
        return new ReadmeCategory(
            module.Metadata.Name,
            libraries,
            overviews,
            features,
            rating);
    }

    private static string Place(int place) => place switch
    {
        1 => "gold",
        2 => "silver",
        _ => "bronze"
    };

    private string RelativePath(string path) =>
        Path.GetRelativePath(buildPaths.SolutionDirectory, path).Replace('\\', '/');

    private static T Read<T>(string path) =>
        JsonSerializer.Deserialize<T>(File.ReadAllText(path), JsonOptions)
        ?? throw new InvalidOperationException($"Cannot read '{path}'.");
}

public sealed record ReadmeModel(
    string ApplicationUrl,
    IReadOnlyList<ReadmeCategory> Categories);

public sealed record ReadmeCategory(
    string Name,
    IReadOnlyList<ReadmeLibrary> Libraries,
    IReadOnlyList<ReadmeChart> Overviews,
    IReadOnlyList<ReadmeFeature> Features,
    IReadOnlyList<ReadmeRating> Rating);

public sealed record ReadmeRating(
    int Place,
    string Name,
    int Gold,
    int Silver,
    int Bronze,
    string Awards);

public sealed record ReadmeLibrary(
    string Name,
    string Version,
    string Description,
    string? Url,
    string Logo);

public sealed record ReadmeChart(
    string Name,
    string Path);

public sealed record ReadmeFeature(
    int Order,
    string Name,
    string? Description,
    string ChartPath);
