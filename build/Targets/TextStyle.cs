using SkiaSharp;

namespace Build.Targets;

internal sealed class TextStyle : IDisposable
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
