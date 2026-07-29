using System.Text.RegularExpressions;
// ReSharper disable CheckNamespace
// ReSharper disable UseCollectionExpression
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Matrix.Web;

/// <summary>
/// <paramref name="Warning"/> is set when the release list could not be read but the
/// application can still work with whatever versions it does have.
/// </summary>
public sealed record MatrixCatalogResult(
    MatrixWebCatalog Catalog,
    string? Warning);

public interface IMatrixDataSource
{
    Task<MatrixCatalogResult> LoadCatalogAsync(CancellationToken cancellationToken = default);

    Task<DateTimeOffset?> LoadVersionDateAsync(
        MatrixWebCatalog catalog,
        MatrixVersion version,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CategoryReport>> LoadAsync(
        MatrixWebCatalog catalog,
        MatrixVersion version,
        IEnumerable<MatrixCategory> categories,
        CancellationToken cancellationToken = default);
}

public sealed record CategoryReport(
    MatrixCategory Category,
    FeatureReport? Features,
    BenchmarkReport? Benchmarks,
    MatrixLibraryMetadataCatalog? LibraryCatalog,
    MatrixChartCatalog? ChartCatalog,
    MatrixFeatureCatalog? FeatureCatalog,
    string? Error);

internal sealed class GitHubMatrixDataSource(
    HttpClient httpClient,
    IWebAssemblyHostEnvironment hostEnvironment,
    NavigationManager navigationManager) : IMatrixDataSource
{
    private static readonly Regex VersionPattern =
        new(@"^\d+\.\d+\.\d+$", RegexOptions.CultureInvariant);

    public async Task<MatrixCatalogResult> LoadCatalogAsync(
        CancellationToken cancellationToken = default)
    {
        var catalog = await httpClient.GetFromJsonAsync<MatrixWebCatalog>(
                          "data/catalog.json",
                          cancellationToken)
                      ?? throw new InvalidOperationException("The matrix catalog is empty.");

        var versions = new List<MatrixVersion>();
        if (UseLocalReports)
        {
            versions.Add(new MatrixVersion("local", null, string.Empty));
        }

        string? warning = null;
        if (catalog.Versions.Count > 0)
        {
            // The build bakes the releases in from the git clone, which keeps the
            // published application off the rate limited GitHub API entirely.
            versions.AddRange(catalog.Versions);
        }
        else
        {
            try
            {
                versions.AddRange(await LoadVersionsAsync(catalog.Repository, cancellationToken));
            }
            catch (Exception exception)
            {
                // Losing the release list must not take down a page that can still
                // render the versions it already has.
                warning = exception.Message;
            }
        }

        return new MatrixCatalogResult(catalog with { Versions = versions }, warning);
    }

    public async Task<IReadOnlyList<CategoryReport>> LoadAsync(
        MatrixWebCatalog catalog,
        MatrixVersion version,
        IEnumerable<MatrixCategory> categories,
        CancellationToken cancellationToken = default)
    {
        var tasks = categories.Select(category =>
            LoadCategoryAsync(catalog.Repository, version, category, cancellationToken));
        return await Task.WhenAll(tasks);
    }

    private async Task<CategoryReport> LoadCategoryAsync(
        GitHubRepository repository,
        MatrixVersion version,
        MatrixCategory category,
        CancellationToken cancellationToken)
    {
        try
        {
            var isLocal = version.Version == "local";
            var repositoryRoot =
                $"https://raw.githubusercontent.com/{repository.Owner}/{repository.Name}/{version.Commit}";
            var reportsRoot = $"{repositoryRoot}/reports/{category.ReportDirectory}";
            var metadataRoot = $"{repositoryRoot}/metadata/{category.ReportDirectory}";
            var featuresTask = isLocal
                ? TryGetLocalAsync<FeatureReport>(
                    "Reports",
                    category,
                    "features.json",
                    cancellationToken)
                : TryGetAsync<FeatureReport>($"{reportsRoot}/features.json", cancellationToken);
            var benchmarksTask = isLocal
                ? TryGetLocalAsync<BenchmarkReport>(
                    "Reports",
                    category,
                    "benchmarks.json",
                    cancellationToken)
                : TryGetAsync<BenchmarkReport>($"{reportsRoot}/benchmarks.json", cancellationToken);
            var libraryCatalogTask = isLocal
                ? TryGetLocalAsync<MatrixLibraryMetadataCatalog>(
                    "Metadata",
                    category,
                    "libraries.json",
                    cancellationToken)
                : TryGetAsync<MatrixLibraryMetadataCatalog>(
                    $"{metadataRoot}/libraries.json",
                    cancellationToken);
            var chartCatalogTask = isLocal
                ? TryGetLocalAsync<MatrixChartCatalog>(
                    "Metadata",
                    category,
                    "charts.json",
                    cancellationToken)
                : TryGetAsync<MatrixChartCatalog>(
                    $"{metadataRoot}/charts.json",
                    cancellationToken);
            var featureCatalogTask = isLocal
                ? TryGetLocalAsync<MatrixFeatureCatalog>(
                    "Metadata",
                    category,
                    "features.json",
                    cancellationToken)
                : TryGetAsync<MatrixFeatureCatalog>(
                    $"{metadataRoot}/features.json",
                    cancellationToken);
            await Task.WhenAll(
                featuresTask,
                benchmarksTask,
                libraryCatalogTask,
                chartCatalogTask,
                featureCatalogTask);
            var features = await featuresTask;
            var benchmarks = await benchmarksTask;
            var libraryCatalog = await ResolveLogosAsync(
                await libraryCatalogTask,
                isLocal,
                category,
                metadataRoot,
                cancellationToken);
            var error = features is null && benchmarks is null
                ? "No reports were published for this category and version."
                : null;
            return new CategoryReport(
                category,
                features,
                benchmarks,
                libraryCatalog,
                await chartCatalogTask,
                await featureCatalogTask,
                error);
        }
        catch (Exception exception)
        {
            return new CategoryReport(category, null, null, null, null, null, exception.Message);
        }
    }

    private async Task<T?> TryGetAsync<T>(string uri, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(uri, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
    }

    private async Task<IReadOnlyList<MatrixVersion>> LoadVersionsAsync(
        GitHubRepository repository,
        CancellationToken cancellationToken)
    {
        var tags = new List<GitHubTag>();
        for (var page = 1; ; page++)
        {
            var uri =
                $"https://api.github.com/repos/{repository.Owner}/{repository.Name}/tags"
                + $"?per_page=100&page={page}";
            var pageTags = await GetRequiredAsync<GitHubTag[]>(uri, cancellationToken);
            tags.AddRange(pageTags.Where(tag => VersionPattern.IsMatch(tag.Name)));
            if (pageTags.Length < 100)
            {
                break;
            }
        }

        return tags
            .Select(tag => (
                Value: new MatrixVersion(tag.Name, null, tag.Commit.Sha),
                SortKey: Version.Parse(tag.Name)))
            .OrderByDescending(version => version.SortKey)
            .Select(version => version.Value)
            .ToArray();
    }

    public async Task<DateTimeOffset?> LoadVersionDateAsync(
        MatrixWebCatalog catalog,
        MatrixVersion version,
        CancellationToken cancellationToken = default)
    {
        if (version.Commit.Length == 0)
        {
            return null;
        }

        try
        {
            var uri =
                $"https://api.github.com/repos/{catalog.Repository.Owner}/{catalog.Repository.Name}"
                + $"/commits/{Uri.EscapeDataString(version.Commit)}";
            var commit = await GetRequiredAsync<GitHubCommit>(uri, cancellationToken);
            return commit.Commit.Committer.Date;
        }
        catch (Exception)
        {
            // A missing date only removes a caption; it must not break the page.
            return null;
        }
    }

    private async Task<T> GetRequiredAsync<T>(
        string uri,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.Accept.ParseAdd("application/vnd.github+json");
        request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");
        using var response = await httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(Describe(response));
        }

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken)
               ?? throw new InvalidOperationException($"GitHub returned an empty response for '{uri}'.");
    }

    /// <summary>
    /// The trimmed runtime strips the resource strings behind EnsureSuccessStatusCode,
    /// so a failure would otherwise surface as an unreadable resource key.
    /// </summary>
    private static string Describe(HttpResponseMessage response)
    {
        var rateLimited =
            response.StatusCode is System.Net.HttpStatusCode.Forbidden
                or System.Net.HttpStatusCode.TooManyRequests
            && response.Headers.TryGetValues("X-RateLimit-Remaining", out var remaining)
            && remaining.FirstOrDefault() == "0";
        if (!rateLimited)
        {
            return $"GitHub returned {(int)response.StatusCode} {response.StatusCode} "
                   + $"for '{response.RequestMessage?.RequestUri}'.";
        }

        var reset = response.Headers.TryGetValues("X-RateLimit-Reset", out var values)
                    && long.TryParse(values.FirstOrDefault(), out var seconds)
            ? DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime().ToString("HH:mm")
            : null;
        return "The GitHub API rate limit for this network has been reached"
               + (reset is null ? "." : $"; it resets at {reset}.")
               + " Released reports stay available once the limit resets.";
    }

    private static async Task<T?> TryGetLocalAsync<T>(
        string resourceGroup,
        MatrixCategory category,
        string fileName,
        CancellationToken cancellationToken)
    {
        await using var stream = GetLocalResourceStream(
            $"Matrix.Web.{resourceGroup}/{category.ReportDirectory}/{fileName}");
        return stream is null
            ? default
            : await JsonSerializer.DeserializeAsync<T>(
                stream,
                new JsonSerializerOptions(JsonSerializerDefaults.Web),
                cancellationToken);
    }

    private static async Task<MatrixLibraryMetadataCatalog?> ResolveLogosAsync(
        MatrixLibraryMetadataCatalog? catalog,
        bool isLocal,
        MatrixCategory category,
        string metadataRoot,
        CancellationToken cancellationToken)
    {
        if (catalog is null)
        {
            return null;
        }

        var libraries = new List<MatrixLibraryMetadata>(catalog.Libraries.Count);
        foreach (var library in catalog.Libraries)
        {
            var logo = isLocal
                ? await GetLocalLogoAsync(category, library.Logo, cancellationToken)
                : $"{metadataRoot}/{library.Logo}";
            libraries.Add(library with { Logo = logo ?? string.Empty });
        }

        return catalog with { Libraries = libraries };
    }

    private static async Task<string?> GetLocalLogoAsync(
        MatrixCategory category,
        string relativePath,
        CancellationToken cancellationToken)
    {
        await using var stream = GetLocalResourceStream(
            $"Matrix.Web.Metadata/{category.ReportDirectory}/{relativePath}");
        if (stream is null)
        {
            return null;
        }

        using var buffer = new MemoryStream();
        await stream.CopyToAsync(buffer, cancellationToken);
        var mediaType = Path.GetExtension(relativePath).ToLowerInvariant() switch
        {
            ".svg" => "image/svg+xml",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".webp" => "image/webp",
            _ => "image/png"
        };
        return $"data:{mediaType};base64,{Convert.ToBase64String(buffer.ToArray())}";
    }

    private static Stream? GetLocalResourceStream(string resourceName)
    {
        var assembly = typeof(GitHubMatrixDataSource).Assembly;
        var normalizedName = resourceName.Replace('\\', '/');
        var actualName = assembly.GetManifestResourceNames()
            .FirstOrDefault(name =>
                name.Replace('\\', '/').Equals(normalizedName, StringComparison.Ordinal));
        return actualName is null ? null : assembly.GetManifestResourceStream(actualName);
    }

    private bool UseLocalReports =>
        hostEnvironment.IsDevelopment()
        && !GetQueryParameter("source").Equals("github", StringComparison.OrdinalIgnoreCase);

    private string GetQueryParameter(string name)
    {
        var query = navigationManager.ToAbsoluteUri(navigationManager.Uri).Query;
        foreach (var part in query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var pair = part.Split('=', 2);
            if (Uri.UnescapeDataString(pair[0]).Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return pair.Length == 2 ? Uri.UnescapeDataString(pair[1]) : string.Empty;
            }
        }

        return string.Empty;
    }

    private sealed record GitHubTag(
        string Name,
        GitHubTagCommit Commit);

    private sealed record GitHubTagCommit(string Sha);

    private sealed record GitHubCommit(
        string Sha,
        GitHubCommitData Commit);

    private sealed record GitHubCommitData(GitHubCommitter Committer);

    private sealed record GitHubCommitter(DateTimeOffset Date);
}
