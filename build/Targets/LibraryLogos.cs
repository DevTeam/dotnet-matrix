using Matrix;
using SkiaSharp;
using Svg.Skia;

namespace Build.Targets;

internal sealed class LibraryLogos : IDisposable
{
    private const int SourceSize = 128;
    private static readonly SKSamplingOptions LogoSampling =
        new(SKCubicResampler.Mitchell);
    private readonly Dictionary<string, SKImage> _images =
        new(StringComparer.OrdinalIgnoreCase);

    public static LibraryLogos Load(
        string metadataDirectory,
        MatrixLibraryMetadataCatalog catalog)
    {
        var logos = new LibraryLogos();
        foreach (var library in catalog.Libraries)
        {
            var path = Path.Combine(metadataDirectory, library.Logo);
            var image = Decode(path);
            if (image is null)
            {
                Console.Error.WriteLine(
                    $"WARNING: Cannot read the logo of '{library.Id}': {path}");
                continue;
            }

            logos._images[library.Id] = image;
        }

        return logos;
    }

    public SKImage? Find(string libraryId) => _images.GetValueOrDefault(libraryId);

    public void Dispose()
    {
        foreach (var image in _images.Values)
        {
            image.Dispose();
        }

        _images.Clear();
    }

    private static SKImage? Decode(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        if (Path.GetExtension(path).Equals(".svg", StringComparison.OrdinalIgnoreCase))
        {
            using var svg = new SKSvg();
            return svg.Load(path) is { } picture
                ? Normalize(picture.CullRect, canvas => canvas.DrawPicture(picture))
                : null;
        }

        using var bitmap = SKBitmap.Decode(path);
        return bitmap is null
            ? null
            : Normalize(
                SKRect.Create(bitmap.Width, bitmap.Height),
                // ReSharper disable once AccessToDisposedClosure
                canvas => canvas.DrawBitmap(bitmap, 0, 0, LogoSampling));
    }

    /// <summary>
    /// Draws the source into a transparent square, scaled to fit and centred,
    /// so that every logo can later be blitted into the same box on a chart.
    /// </summary>
    private static SKImage? Normalize(SKRect source, Action<SKCanvas> draw)
    {
        if (source.Width <= 0 || source.Height <= 0)
        {
            return null;
        }

        using var surface = SKSurface.Create(
            new SKImageInfo(SourceSize, SourceSize, SKColorType.Rgba8888, SKAlphaType.Premul));
        if (surface is null)
        {
            return null;
        }

        var canvas = surface.Canvas;
        canvas.Clear(SKColors.Transparent);
        var scale = Math.Min(SourceSize / source.Width, SourceSize / source.Height);
        canvas.Translate(
            (SourceSize - source.Width * scale) / 2,
            (SourceSize - source.Height * scale) / 2);
        canvas.Scale(scale);
        canvas.Translate(-source.Left, -source.Top);
        draw(canvas);
        return surface.Snapshot();
    }
}
