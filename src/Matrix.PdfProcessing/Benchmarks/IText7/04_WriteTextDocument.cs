using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteTextDocument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IText7)]
    public byte[] IText7()
    {
        using var output = new MemoryStream();
        using (var document = new PdfDocument(new PdfWriter(output)))
        {
            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var layout = new iText.Layout.Document(document, iText.Kernel.Geom.PageSize.A4);
            layout.Add(new iText.Layout.Element.Paragraph(PdfModel.Heading)
                .SetFont(font)
                .SetFontSize((float)PdfPage.HeadingFontSize));
            foreach (var line in PdfModel.TextDocument)
            {
                if (!string.Equals(line, PdfModel.Heading, StringComparison.Ordinal))
                {
                    layout.Add(new iText.Layout.Element.Paragraph(line)
                        .SetFont(font)
                        .SetFontSize((float)PdfPage.BodyFontSize));
                }
            }

            layout.Close();
        }

        var bytes = output.ToArray();

        PdfChecks.Document(LibraryCatalog.IText7, bytes, 1, PdfModel.TextDocument);
        return bytes;
    }
}
