using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Matrix.PdfProcessing;

/// <summary>
/// The single deterministic reading fixture every reader receives. It is part of
/// the category contract, not an implementation detail: one reader's page count
/// is only comparable with another's when both opened the same bytes.
/// See <c>workflows/feature-contracts/pdf-processing.md</c>.
/// </summary>
internal static class PdfFixture
{
    public const string Title = "Matrix PDF Fixture";

    public const string Author = "dotnet-matrix";

    public const string Subject = "Deterministic reading fixture";

    public const string Heading = "Page 1 of the deterministic fixture.";

    public const int PageCount = 3;

    public const int BodyLineCount = 20;

    private static readonly Lazy<byte[]> Value = new(Create);

    public static byte[] Bytes => Value.Value;

    /// <summary>
    /// The fixture is written with iText7's standard-14 Helvetica, whose font
    /// resource declares <c>/WinAnsiEncoding</c>. PdfPig's writer, in contrast,
    /// emits <c>/StandardEncoding</c>, which iText7's text extractor refuses with
    /// "not a supported encoding name" — so a PdfPig-written fixture would fail
    /// one participant for a reason that has nothing to do with its performance.
    /// Validation opens this fixture with every reader in the category.
    /// </summary>
    private static byte[] Create()
    {
        using var output = new MemoryStream();
        using (var document = new PdfDocument(new PdfWriter(output)))
        {
            document.GetDocumentInfo()
                .SetTitle(Title)
                .SetAuthor(Author)
                .SetSubject(Subject)
                .SetCreator(Author);
            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var layout = new Document(document, PageSize.A4);
            layout.SetMargins(
                (float)PdfPage.Margin,
                (float)PdfPage.Margin,
                (float)PdfPage.Margin,
                (float)PdfPage.Margin);
            for (var pageNumber = 1; pageNumber <= PageCount; pageNumber++)
            {
                if (pageNumber > 1)
                {
                    layout.Add(new AreaBreak());
                }

                layout.Add(new Paragraph($"Page {pageNumber} of the deterministic fixture.")
                    .SetFont(font)
                    .SetFontSize((float)PdfPage.HeadingFontSize));
                for (var line = 0; line < BodyLineCount; line++)
                {
                    layout.Add(new Paragraph($"Row {line:D2}: the quick brown fox jumps over the lazy dog.")
                        .SetFont(font)
                        .SetFontSize((float)PdfPage.BodyFontSize));
                }
            }

            layout.Close();
        }

        return output.ToArray();
    }
}
