namespace Matrix.PdfProcessing;

/// <summary>
/// Serves one system font program to PDFsharp under a fixed family name, so the
/// writing scenarios never depend on which families the current machine lists.
/// </summary>
internal sealed class PdfSystemFontResolver(string path) : PdfSharp.Fonts.IFontResolver
{
    public const string Family = "Matrix Sans";

    private const string FaceName = "MatrixSans#Regular";

    private byte[]? program;

    public PdfSharp.Fonts.FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic) =>
        new(FaceName);

    public byte[] GetFont(string faceName) =>
        program ??= File.ReadAllBytes(path);
}
