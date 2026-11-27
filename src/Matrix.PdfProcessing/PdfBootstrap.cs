namespace Matrix.PdfProcessing;

/// <summary>
/// One-time, process-wide setup that the PDF libraries need before any call.
/// See <c>workflows/feature-contracts/pdf-processing.md</c>.
/// </summary>
internal static class PdfBootstrap
{
    private static readonly Lazy<bool> Applied = new(Apply);

    private static readonly Lazy<string?> FontPath = new(FindFontPath);

    /// <summary>
    /// The system font program the PDFsharp writer resolves, or <c>null</c> when
    /// this machine has none of the candidate files. A missing font fails the
    /// PDFsharp writing scenarios with a factual reason instead of publishing a
    /// claim about the library that would be false on a machine that has one.
    /// </summary>
    public static string? FontProgramPath => FontPath.Value;

    public static void EnsureConfigured()
    {
        _ = Applied.Value;
    }

    private static bool Apply()
    {
        // PDFsharp 6.5 refuses every operation until its licence is accepted, and
        // the category accepts the AGPL for PDFsharp and iText7.
        PdfSharp.PdfSharpLicense.SetAgplLicense();

        // QuestPDF 2026 requires an explicit licence selection before first use.
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        if (FontPath.Value is { } path)
        {
            PdfSharp.Fonts.GlobalFontSettings.FontResolver = new PdfSystemFontResolver(path);
        }

        // Docnet.Core loads its native PDFium binary on first use. Doing it here keeps
        // that failure out of a measured method and out of an unrelated scenario.
        try
        {
            using var reader = Docnet.Core.DocLib.Instance.GetDocReader(
                PdfFixture.Bytes,
                new Docnet.Core.Models.PageDimensions(1.0));
            _ = reader.GetPageCount();
        }
        catch (Exception)
        {
            // A platform without a PDFium build reports its own failure through the
            // reading scenarios; nothing here may stop the rest of the category.
        }

        return true;
    }

    private static string? FindFontPath()
    {
        string[] candidates =
        [
            @"C:\Windows\Fonts\arial.ttf",
            @"C:\Windows\Fonts\segoeui.ttf",
            "/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf",
            "/usr/share/fonts/truetype/liberation/LiberationSans-Regular.ttf",
            "/usr/share/fonts/TTF/DejaVuSans.ttf",
            "/usr/share/fonts/liberation-sans/LiberationSans-Regular.ttf",
            "/usr/share/fonts/truetype/noto/NotoSans-Regular.ttf",
            "/System/Library/Fonts/Supplemental/Arial.ttf"
        ];

        return Array.Find(candidates, File.Exists);
    }
}
