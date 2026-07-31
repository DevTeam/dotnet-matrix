using SkiaSharp;
using Svg.Skia;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Build.Targets;

/// <summary>
/// Makes the Web application installable: the manifest Chrome reads and the raster
/// icons it insists on. Both are generated into the source tree beside
/// <c>icon.svg</c>, so the local application is installable too and the published
/// artifact needs no extra step.
/// </summary>
internal sealed class WebManifestTarget(IBuildPaths buildPaths) : IWebManifestTarget
{
    private const string ManifestFileName = "manifest.webmanifest";
    private const string SourceIconFileName = "icon.svg";

    /// <summary>The page background, so a masked icon has no transparent corners.</summary>
    private static readonly SKColor Background = SKColor.Parse("#0b0d12");

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };

    public int Run(IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        var root = Path.Combine(
            buildPaths.SolutionDirectory,
            "src",
            "Matrix.Web",
            "wwwroot");
        var source = Path.Combine(root, SourceIconFileName);
        if (!File.Exists(source))
        {
            Console.Error.WriteLine($"WARNING: '{source}' is missing, cannot build icons.");
            return 1;
        }

        using var svg = new SKSvg();
        if (svg.Load(source) is not { } picture)
        {
            Console.Error.WriteLine($"WARNING: Cannot read '{source}'.");
            return 1;
        }

        WriteIcon(picture, Path.Combine(root, "icon-192.png"), 192, false);
        WriteIcon(picture, Path.Combine(root, "icon-512.png"), 512, false);
        WriteIcon(picture, Path.Combine(root, "icon-512-maskable.png"), 512, true);

        var manifestPath = Path.Combine(root, ManifestFileName);
        File.WriteAllText(
            manifestPath,
            JsonSerializer.Serialize(Create(modules), SerializerOptions) + Environment.NewLine);
        Host.Info($"Web manifest: {manifestPath}");
        return 0;
    }

    /// <summary>
    /// Every address here is relative. The same artifact answers on the custom domain
    /// and on a project page under a repository path, and an absolute start url would
    /// only be right for one of them.
    /// </summary>
    private static WebManifest Create(IReadOnlyList<DiscoveredMatrixModule> modules) =>
        new(
            "./",
            ".NET Matrix",
            "Matrix",
            "Compare .NET libraries by features, performance, and memory.",
            "./",
            "./",
            "standalone",
            "#0b0d12",
            "#11131a",
            false,
            [
                new WebManifestIcon("icon-192.png", "image/png", "192x192", null),
                new WebManifestIcon("icon-512.png", "image/png", "512x512", null),
                new WebManifestIcon("icon-512-maskable.png", "image/png", "512x512", "maskable")
            ],
            // A jump list on the installed icon, one entry per category, through the
            // same query parameter a published link uses.
            modules
                .Select(module => module.Metadata)
                .OrderBy(metadata => metadata.Name, StringComparer.OrdinalIgnoreCase)
                .Select(metadata => new WebManifestShortcut(
                    metadata.Name,
                    metadata.Name,
                    $"./?category={metadata.Id}"))
                .ToArray());

    /// <summary>
    /// A maskable icon is cropped by the platform to whatever shape it likes, so its
    /// artwork keeps to the middle 60% and the rest is filled background. The plain
    /// icon is drawn edge to edge: the source already carries its own rounded plate.
    /// </summary>
    private static void WriteIcon(SKPicture picture, string path, int size, bool maskable)
    {
        using var surface = SKSurface.Create(
            new SKImageInfo(size, size, SKColorType.Rgba8888, SKAlphaType.Premul));
        var canvas = surface.Canvas;
        canvas.Clear(maskable ? Background : SKColors.Transparent);
        var bounds = picture.CullRect;
        var inset = maskable ? size * 0.2f : 0f;
        var box = size - inset * 2;
        var scale = Math.Min(box / bounds.Width, box / bounds.Height);
        canvas.Translate(
            inset + (box - bounds.Width * scale) / 2,
            inset + (box - bounds.Height * scale) / 2);
        canvas.Scale(scale);
        canvas.Translate(-bounds.Left, -bounds.Top);
        canvas.DrawPicture(picture);
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.Create(path);
        data.SaveTo(stream);
    }
}
