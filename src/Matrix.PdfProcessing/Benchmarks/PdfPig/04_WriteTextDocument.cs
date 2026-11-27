using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteTextDocument
{
    /// <summary>
    /// PdfPig builds documents with its standard-14 writer, which needs no font
    /// program, so it draws the same model with the same coordinates as the other
    /// writers and stays comparable on a machine without any system font.
    /// </summary>
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfPig)]
    public byte[] PdfPig()
    {
        var builder = new PdfDocumentBuilder();
        var font = builder.AddStandard14Font(Standard14Font.Helvetica);
        var page = builder.AddPage(PdfPage.Width, PdfPage.Height);
        page.AddText(
            PdfModel.Heading,
            PdfPage.HeadingFontSize,
            new PdfPoint(PdfPage.Margin, PdfPage.Height - PdfPage.Margin),
            font);
        var offset = PdfPage.Height - PdfPage.Margin - (PdfPage.HeadingFontSize * 2);
        foreach (var line in PdfModel.TextDocument)
        {
            if (string.Equals(line, PdfModel.Heading, StringComparison.Ordinal))
            {
                continue;
            }

            offset -= PdfPage.Leading;
            page.AddText(line, PdfPage.BodyFontSize, new PdfPoint(PdfPage.Margin, offset), font);
        }

        var bytes = builder.Build();

        PdfChecks.Document(LibraryCatalog.PdfPig, bytes, 1, PdfModel.TextDocument);
        return bytes;
    }
}
