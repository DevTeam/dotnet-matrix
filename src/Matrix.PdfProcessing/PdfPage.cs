namespace Matrix.PdfProcessing;

/// <summary>
/// A4 geometry shared by every writer and asserted by every reader, so a page
/// size difference is a failure rather than a silent formatting choice.
/// </summary>
internal static class PdfPage
{
    public const double Width = 595;

    public const double Height = 842;

    public const double Margin = 40;

    public const double HeadingFontSize = 16;

    public const double BodyFontSize = 11;

    public const double Leading = 14;

    public const double TableRowHeight = 18;
}
