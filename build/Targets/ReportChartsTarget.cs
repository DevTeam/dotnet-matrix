using Matrix;
using SkiaSharp;
using Svg.Skia;
using System.Text.Json;
// ReSharper disable UseCollectionExpression

namespace Build.Targets;

internal interface IReportChartsTarget
{
    int Run(IReadOnlyList<DiscoveredMatrixModule> modules);
}

internal sealed class ReportChartsTarget(IBuildPaths buildPaths) : IReportChartsTarget
{
    private const int ImageWidth = 1400;
    private const float LabelWidth = 404;
    private const float OuterPadding = 36;
    private const float PanelGap = 28;
    private const float BarHeight = 22;
    private const float LogoSize = 28;
    private const float LogoGap = 10;
    private const float LabelTextX = OuterPadding + LogoSize + LogoGap;
    private const float LabelTextWidth = LabelWidth - 24 - LogoSize - LogoGap;
    private static readonly SKColor Background = SKColor.Parse("#0B0D12");
    private static readonly SKColor Surface = SKColor.Parse("#11141B");
    private static readonly SKColor Surface2 = SKColor.Parse("#171B24");
    private static readonly SKColor Line = SKColor.Parse("#292F3A");
    private static readonly SKColor Text = SKColor.Parse("#F2F4F8");
    private static readonly SKColor Muted = SKColor.Parse("#8E98A8");
    private static readonly SKColor Performance = SKColor.Parse("#68D8EF");
    private static readonly SKColor Memory = SKColor.Parse("#B7F34A");
    private static readonly SKColor[] FeatureColors =
    [
        SKColor.Parse("#68D8EF"),
        SKColor.Parse("#B7F34A"),
        SKColor.Parse("#B86BE3"),
        SKColor.Parse("#FF7B7F"),
        SKColor.Parse("#F4BD50"),
        SKColor.Parse("#57D6B9"),
        SKColor.Parse("#7F9CFF"),
        SKColor.Parse("#E78CC8")
    ];
    private static readonly SKSamplingOptions LogoSampling =
        new(SKCubicResampler.Mitchell);
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public int Run(IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        foreach (var module in modules)
        {
            Render(module);
        }

        return 0;
    }

    private void Render(DiscoveredMatrixModule module)
    {
        var reportDirectory = Path.Combine(
            buildPaths.SolutionDirectory,
            "reports",
            module.Metadata.ReportDirectory);
        var reportPath = Path.Combine(reportDirectory, "benchmarks.json");
        var metadataDirectory = Path.Combine(
            buildPaths.SolutionDirectory,
            "metadata",
            module.Metadata.ReportDirectory);
        var catalogPath = Path.Combine(metadataDirectory, "charts.json");
        if (!File.Exists(reportPath) || !File.Exists(catalogPath))
        {
            Console.Error.WriteLine(
                $"WARNING: Cannot render charts for {module.Metadata.Name}: "
                + "benchmarks.json or charts.json is missing.");
            return;
        }

        var report = Read<BenchmarkReport>(reportPath);
        var catalog = Read<MatrixChartCatalog>(catalogPath);
        var chartsDirectory = Path.Combine(reportDirectory, MatrixChartPaths.DirectoryName);
        Directory.CreateDirectory(chartsDirectory);
        using var logos = LibraryLogos.Load(metadataDirectory, module.Metadata.LibraryMetadata);

        foreach (var feature in report.Features.OrderBy(feature => feature.Order))
        {
            var path = Path.Combine(chartsDirectory, MatrixChartPaths.Feature(feature));
            RenderFeature(module.Metadata.Name, report, feature, logos, path);
            Console.WriteLine($"Benchmark chart: {path}");
        }

        foreach (var group in catalog.Groups)
        {
            var features = group.Features
                .Select(id => report.Features.FirstOrDefault(feature =>
                    feature.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
                .Where(feature => feature is not null)
                .Select(feature => feature!)
                .ToArray();
            if (features.Length == 0)
            {
                continue;
            }

            var path = Path.Combine(chartsDirectory, MatrixChartPaths.Overview(group));
            RenderOverview(report, group, features, logos, path);
            Console.WriteLine($"Benchmark overview: {path}");
        }
    }

    private static T Read<T>(string path) =>
        JsonSerializer.Deserialize<T>(File.ReadAllText(path), JsonOptions)
        ?? throw new InvalidOperationException($"Cannot read '{path}'.");

    private static void RenderFeature(
        string category,
        BenchmarkReport report,
        BenchmarkReportEntry feature,
        LibraryLogos logos,
        string outputPath)
    {
        var names = report.Libraries.ToDictionary(
            library => library.Id,
            library => library.Name,
            StringComparer.OrdinalIgnoreCase);
        var rows = feature.Results
            .Where(result => result.Successful)
            .Where(result =>
                result.MeanNanoseconds is not null
                || result.AllocatedBytesPerOperation is not null)
            .OrderBy(result => result.MeanNanoseconds ?? double.MaxValue)
            .ThenBy(result => names.GetValueOrDefault(result.LibraryId, result.LibraryId))
            .ToArray();
        var height = Math.Max(360, 154 + rows.Length * 58);
        using var surface = CreateSurface(height);
        var canvas = surface.Canvas;
        canvas.Clear(Background);

        using var title = TextStyle.Create(Text, 29, true);
        using var subtitle = TextStyle.Create(Muted, 15);
        using var label = TextStyle.Create(Text, 15, true);
        using var value = TextStyle.Create(Text, 14, true);
        using var hint = TextStyle.Create(Muted, 13);
        using var performance = Fill(Performance);
        using var memory = Fill(Memory);
        using var track = Fill(Surface2);

        DrawText(canvas, feature.Name, OuterPadding, 47, title);
        DrawText(canvas, $"{category} · lower is better", OuterPadding, 74, subtitle);

        const float contentWidth = ImageWidth - OuterPadding * 2 - LabelWidth - PanelGap;
        const float panelWidth = contentWidth / 2;
        const float performanceX = OuterPadding + LabelWidth;
        const float memoryX = performanceX + panelWidth + PanelGap;
        DrawPanelHeading(canvas, "PERFORMANCE", "mean execution time", performanceX, panelWidth, subtitle, hint);
        DrawPanelHeading(canvas, "MEMORY", "allocated / operation", memoryX, panelWidth, subtitle, hint);

        var maximumTime = rows.Select(row => row.MeanNanoseconds ?? 0).DefaultIfEmpty().Max();
        var maximumMemory = rows.Select(row => row.AllocatedBytesPerOperation ?? 0).DefaultIfEmpty().Max();
        for (var index = 0; index < rows.Length; index++)
        {
            var row = rows[index];
            var y = 132 + index * 58;
            DrawStripe(canvas, index, y, 48);
            var libraryName = names.GetValueOrDefault(row.LibraryId, row.LibraryId);
            DrawLogo(canvas, logos.Find(row.LibraryId), libraryName, y - 1);
            DrawFittedText(
                canvas,
                $"{index + 1}. {libraryName}",
                LabelTextX,
                y + 5,
                LabelTextWidth,
                label);
            DrawMetric(
                canvas,
                performanceX,
                y,
                panelWidth,
                row.MeanNanoseconds,
                maximumTime,
                FormatTime,
                performance,
                track,
                value);
            DrawMetric(
                canvas,
                memoryX,
                y,
                panelWidth,
                row.AllocatedBytesPerOperation,
                maximumMemory,
                FormatBytes,
                memory,
                track,
                value);
        }

        Save(surface, outputPath);
    }

    private static void RenderOverview(
        BenchmarkReport report,
        MatrixChartGroup group,
        IReadOnlyList<BenchmarkReportEntry> features,
        LibraryLogos logos,
        string outputPath)
    {
        var names = report.Libraries.ToDictionary(
            library => library.Id,
            library => library.Name,
            StringComparer.OrdinalIgnoreCase);
        var rows = report.Libraries
            .Select(library => CreateOverviewRow(library, features))
            .ToArray();
        var rankedRows = rows
            .Where(row => row.MissingFeatures.Count == 0)
            .OrderBy(row => row.PerformanceValues.Sum())
            .ThenBy(row => names.GetValueOrDefault(row.LibraryId, row.LibraryId))
            .ToArray();
        var unrankedRows = rows
            .Where(row => row.MissingFeatures.Count > 0)
            .OrderBy(row => row.MissingFeatures.Count)
            .ThenBy(row => names.GetValueOrDefault(row.LibraryId, row.LibraryId))
            .ToArray();
        var legendRows = (int)Math.Ceiling(features.Count / 3d);
        var unrankedHeight = unrankedRows.Length == 0
            ? 0
            : 38 + unrankedRows.Length * 72;
        var height = Math.Max(
            420,
            176
            + rankedRows.Length * 62
            + unrankedHeight
            + legendRows * 30);
        using var surface = CreateSurface(height);
        var canvas = surface.Canvas;
        canvas.Clear(Background);

        using var title = TextStyle.Create(Text, 29, true);
        using var subtitle = TextStyle.Create(Muted, 15);
        using var label = TextStyle.Create(Text, 15, true);
        using var value = TextStyle.Create(Text, 14, true);
        using var hint = TextStyle.Create(Muted, 13);
        using var partial = TextStyle.Create(Muted, 12, true);
        using var track = Fill(Surface2);

        DrawText(canvas, $"{group.Name} overview", OuterPadding, 47, title);
        DrawText(
            canvas,
            $"{features.Count}/{features.Count} coverage required for ranking · lower is better",
            OuterPadding,
            74,
            subtitle);

        var contentWidth = ImageWidth - OuterPadding * 2 - LabelWidth - PanelGap;
        var panelWidth = contentWidth / 2;
        var performanceX = OuterPadding + LabelWidth;
        var memoryX = performanceX + panelWidth + PanelGap;
        DrawPanelHeading(canvas, "PERFORMANCE", "total mean time", performanceX, panelWidth, subtitle, hint);
        DrawPanelHeading(canvas, "MEMORY", "total allocated", memoryX, panelWidth, subtitle, hint);

        var scaleRows = rankedRows.Length > 0
            ? rankedRows
            : unrankedRows;
        var maximumTime = scaleRows
            .Select(row => row.PerformanceValues.Sum(i => i ?? 0))
            .DefaultIfEmpty()
            .Max();
        var maximumMemory = scaleRows
            .Select(row => row.MemoryValues.Sum(i => i ?? 0))
            .DefaultIfEmpty()
            .Max();
        var y = 132f;
        var stripeIndex = 0;
        for (var index = 0; index < rankedRows.Length; index++)
        {
            var row = rankedRows[index];
            var rankedName = names.GetValueOrDefault(row.LibraryId, row.LibraryId);
            DrawStripe(canvas, stripeIndex++, y, 50);
            DrawLogo(canvas, logos.Find(row.LibraryId), rankedName, y);
            DrawFittedText(
                canvas,
                $"{index + 1}. {rankedName}",
                LabelTextX,
                y + 5,
                LabelTextWidth,
                label);
            DrawStackedMetric(
                canvas,
                performanceX,
                y,
                panelWidth,
                row.PerformanceValues,
                maximumTime,
                FormatTime,
                track,
                value);
            DrawStackedMetric(
                canvas,
                memoryX,
                y,
                panelWidth,
                row.MemoryValues,
                maximumMemory,
                FormatBytes,
                track,
                value);
            y += 62;
        }

        if (unrankedRows.Length > 0)
        {
            DrawText(canvas, "NOT RANKED · PARTIAL COVERAGE", OuterPadding, y + 4, partial);
            using var separator = Fill(Line);
            canvas.DrawRect(
                OuterPadding + 224,
                y - 4,
                ImageWidth - OuterPadding * 2 - 224,
                1,
                separator);
            y += 38;

            foreach (var row in unrankedRows)
            {
                var unrankedName = names.GetValueOrDefault(row.LibraryId, row.LibraryId);
                DrawStripe(canvas, stripeIndex++, y, 62);
                DrawLogo(canvas, logos.Find(row.LibraryId), unrankedName, y + 6);
                DrawFittedText(
                    canvas,
                    unrankedName,
                    LabelTextX,
                    y - 3,
                    LabelTextWidth,
                    label);
                var coverage = features.Count - row.MissingFeatures.Count;
                DrawFittedText(
                    canvas,
                    $"{coverage}/{features.Count} · Missing: {string.Join(", ", row.MissingFeatures)}",
                    LabelTextX,
                    y + 18,
                    LabelTextWidth,
                    hint);
                DrawStackedMetric(
                    canvas,
                    performanceX,
                    y,
                    panelWidth,
                    row.PerformanceValues,
                    maximumTime,
                    FormatTime,
                    track,
                    value);
                DrawStackedMetric(
                    canvas,
                    memoryX,
                    y,
                    panelWidth,
                    row.MemoryValues,
                    maximumMemory,
                    FormatBytes,
                    track,
                    value);
                y += 72;
            }
        }

        DrawLegend(canvas, features, y + 20, hint);
        Save(surface, outputPath);
    }

    private static OverviewRow CreateOverviewRow(
        BenchmarkLibrary library,
        IReadOnlyList<BenchmarkReportEntry> features)
    {
        var results = features
            .Select(feature => feature.Results.FirstOrDefault(result =>
                result.Successful
                && result.LibraryId.Equals(library.Id, StringComparison.OrdinalIgnoreCase)))
            .ToArray();
        return new OverviewRow(
            library.Id,
            results.Select(result => result?.MeanNanoseconds).ToArray(),
            results.Select(result => result?.AllocatedBytesPerOperation).ToArray(),
            features
                .Where((_, index) => results[index] is null)
                .Select(feature => feature.Name)
                .ToArray());
    }

    private static void DrawStripe(SKCanvas canvas, int index, float y, float height)
    {
        if (index % 2 != 0)
        {
            return;
        }

        using var stripe = Fill(Surface);
        canvas.DrawRoundRect(
            SKRect.Create(OuterPadding - 10, y - 25, ImageWidth - OuterPadding * 2 + 20, height),
            8,
            8,
            stripe);
    }

    private static void DrawLogo(
        SKCanvas canvas,
        SKImage? image,
        string libraryName,
        float centerY)
    {
        var rect = SKRect.Create(OuterPadding, centerY - LogoSize / 2, LogoSize, LogoSize);
        if (image is not null)
        {
            canvas.Save();
            canvas.ClipRoundRect(new SKRoundRect(rect, 6, 6), antialias: true);
            canvas.DrawImage(image, rect, LogoSampling);
            canvas.Restore();
            return;
        }

        using var fill = Fill(Surface2);
        using var border = new SKPaint
        {
            Color = Line,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1
        };
        canvas.DrawRoundRect(rect, 6, 6, fill);
        canvas.DrawRoundRect(rect, 6, 6, border);

        using var monogram = TextStyle.Create(Muted, 14, true);
        var letter = libraryName.Length > 0
            ? libraryName[..1].ToUpperInvariant()
            : "?";
        canvas.DrawText(
            letter,
            rect.MidX - monogram.Font.MeasureText(letter) / 2,
            rect.MidY + 5,
            SKTextAlign.Left,
            monogram.Font,
            monogram.Paint);
    }

    private static void DrawPanelHeading(
        SKCanvas canvas,
        string heading,
        string description,
        float x,
        float width,
        TextStyle headingStyle,
        TextStyle descriptionStyle)
    {
        DrawText(canvas, heading, x, 80, headingStyle);
        DrawText(canvas, description, x, 101, descriptionStyle);
        using var line = Fill(Line);
        canvas.DrawRect(x, 108, width, 1, line);
    }

    private static void DrawMetric(
        SKCanvas canvas,
        float x,
        float y,
        float width,
        double? current,
        double maximum,
        Func<double, string> formatter,
        SKPaint bar,
        SKPaint track,
        TextStyle value)
    {
        const float valueWidth = 106;
        var barWidth = width - valueWidth - 12;
        canvas.DrawRoundRect(SKRect.Create(x, y - 12, barWidth, BarHeight), 7, 7, track);
        if (current is not null)
        {
            var filled = Scale(current.Value, maximum, barWidth);
            canvas.DrawRoundRect(SKRect.Create(x, y - 12, filled, BarHeight), 7, 7, bar);
            DrawText(canvas, formatter(current.Value), x + barWidth + 12, y + 5, value);
        }
        else
        {
            DrawText(canvas, "n/a", x + barWidth + 12, y + 5, value);
        }
    }

    private static void DrawStackedMetric(
        SKCanvas canvas,
        float x,
        float y,
        float width,
        IReadOnlyList<double?> values,
        double maximum,
        Func<double, string> formatter,
        SKPaint track,
        TextStyle value)
    {
        const float valueWidth = 106;
        var barWidth = width - valueWidth - 12;
        var hasValues = values.Any(i => i is not null);
        var total = values.Sum(i => i ?? 0);
        canvas.DrawRoundRect(SKRect.Create(x, y - 12, barWidth, BarHeight), 7, 7, track);
        if (!hasValues)
        {
            DrawText(canvas, "n/a", x + barWidth + 12, y + 5, value);
            return;
        }

        var filled = Scale(total, maximum, barWidth);
        if (total == 0)
        {
            using var zero = Fill(FeatureColors[0]);
            canvas.DrawCircle(x + 4, y - 1, 4, zero);
        }
        else
        {
            var currentX = x;
            for (var index = 0; index < values.Count; index++)
            {
                var segmentWidth = (float)(filled * (values[index] ?? 0) / total);
                if (segmentWidth <= 0)
                {
                    continue;
                }

                using var segment = Fill(FeatureColors[index % FeatureColors.Length]);
                canvas.DrawRect(currentX, y - 12, segmentWidth, BarHeight, segment);
                currentX += segmentWidth;
            }
        }

        DrawText(canvas, formatter(total), x + barWidth + 12, y + 5, value);
    }

    private static void DrawLegend(
        SKCanvas canvas,
        IReadOnlyList<BenchmarkReportEntry> features,
        float startY,
        TextStyle text)
    {
        const float itemWidth = 420;
        for (var index = 0; index < features.Count; index++)
        {
            var column = index % 3;
            var row = index / 3;
            var x = OuterPadding + column * itemWidth;
            var y = startY + row * 30;
            using var color = Fill(FeatureColors[index % FeatureColors.Length]);
            canvas.DrawRoundRect(SKRect.Create(x, y - 12, 16, 16), 3, 3, color);
            DrawFittedText(canvas, features[index].Name, x + 24, y + 1, itemWidth - 34, text);
        }
    }

    private static float Scale(double current, double maximum, float width)
    {
        if (current <= 0)
        {
            return 7;
        }

        if (maximum <= 0)
        {
            return width;
        }

        return Math.Min(
            width,
            Math.Max(7, (float)(Math.Log10(current + 1) / Math.Log10(maximum + 1) * width)));
    }

    private static void DrawFittedText(
        SKCanvas canvas,
        string text,
        float x,
        float y,
        float maximumWidth,
        TextStyle style)
    {
        if (style.Font.MeasureText(text) <= maximumWidth)
        {
            DrawText(canvas, text, x, y, style);
            return;
        }

        var value = text;
        while (value.Length > 1 && style.Font.MeasureText($"{value}…") > maximumWidth)
        {
            value = value[..^1];
        }

        DrawText(canvas, $"{value}…", x, y, style);
    }

    private static SKSurface CreateSurface(int height) =>
        SKSurface.Create(new SKImageInfo(ImageWidth, height, SKColorType.Rgba8888, SKAlphaType.Premul))
        ?? throw new InvalidOperationException("Cannot create chart surface.");

    private static SKPaint Fill(SKColor color) =>
        new()
        {
            Color = color,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

    private static void Save(SKSurface surface, string outputPath)
    {
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = File.Create(outputPath);
        data.SaveTo(stream);
    }

    private static void DrawText(
        SKCanvas canvas,
        string text,
        float x,
        float y,
        TextStyle style) =>
        canvas.DrawText(
            text,
            x,
            y,
            SKTextAlign.Left,
            style.Font,
            style.Paint);

    private static string FormatTime(double nanoseconds) => nanoseconds switch
    {
        0 => "0 ns",
        < 1_000 => $"{nanoseconds:0.##} ns",
        < 1_000_000 => $"{nanoseconds / 1_000:0.##} μs",
        _ => $"{nanoseconds / 1_000_000:0.##} ms"
    };

    private static string FormatBytes(double bytes) => bytes switch
    {
        0 => "0 B",
        < 1_024 => $"{bytes:0.##} B",
        < 1_048_576 => $"{bytes / 1_024:0.##} KB",
        _ => $"{bytes / 1_048_576:0.##} MB"
    };

    private sealed class LibraryLogos : IDisposable
    {
        private const int SourceSize = 128;

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

    private sealed record OverviewRow(
        string LibraryId,
        IReadOnlyList<double?> PerformanceValues,
        IReadOnlyList<double?> MemoryValues,
        IReadOnlyList<string> MissingFeatures);

    private sealed class TextStyle : IDisposable
    {
        private TextStyle(SKColor color, float size, bool bold)
        {
            Font = new SKFont(
                SKTypeface.FromFamilyName(
                    "Arial",
                    bold ? SKFontStyle.Bold : SKFontStyle.Normal),
                size);
            Paint = new SKPaint
            {
                Color = color,
                IsAntialias = true
            };
        }

        public SKFont Font { get; }

        public SKPaint Paint { get; }

        public static TextStyle Create(SKColor color, float size, bool bold = false) =>
            new(color, size, bold);

        public void Dispose()
        {
            Font.Dispose();
            Paint.Dispose();
        }
    }
}
