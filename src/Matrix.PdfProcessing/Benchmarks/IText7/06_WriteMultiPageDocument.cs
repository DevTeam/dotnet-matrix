using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteMultiPageDocument
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
            for (var pageNumber = 0; pageNumber < PdfModel.MultiPageCount; pageNumber++)
            {
                if (pageNumber > 0)
                {
                    layout.Add(new AreaBreak());
                }

                layout.Add(new iText.Layout.Element.Paragraph(PdfModel.MultiPageHeading(pageNumber))
                    .SetFont(font)
                    .SetFontSize((float)PdfPage.HeadingFontSize));
                for (var line = 0; line < 12; line++)
                {
                    layout.Add(new iText.Layout.Element.Paragraph(PdfModel.MultiPageLine(pageNumber, line))
                        .SetFont(font)
                        .SetFontSize((float)PdfPage.BodyFontSize));
                }
            }

            layout.Close();
        }

        var bytes = output.ToArray();

        PdfChecks.Document(LibraryCatalog.IText7, bytes, PdfModel.MultiPageCount, PdfModel.MultiPage);
        return bytes;
    }
}
