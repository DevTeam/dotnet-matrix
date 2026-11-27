using PdfSharp.Drawing;
using PdfSharp.Pdf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteMultiPageDocument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfSharp)]
    public byte[] PdfSharp()
    {
        PdfChecks.FontAvailable(LibraryCatalog.PdfSharp);
        using var document = new PdfDocument();
        var heading = new XFont(PdfSystemFontResolver.Family, PdfPage.HeadingFontSize);
        var body = new XFont(PdfSystemFontResolver.Family, PdfPage.BodyFontSize);
        for (var pageNumber = 0; pageNumber < PdfModel.MultiPageCount; pageNumber++)
        {
            var page = document.AddPage();
            page.Width = XUnit.FromPoint(PdfPage.Width);
            page.Height = XUnit.FromPoint(PdfPage.Height);
            using var graphics = XGraphics.FromPdfPage(page);
            graphics.DrawString(
                PdfModel.MultiPageHeading(pageNumber),
                heading,
                XBrushes.Black,
                new XPoint(PdfPage.Margin, PdfPage.Margin + PdfPage.HeadingFontSize));
            var offset = PdfPage.Margin + (PdfPage.HeadingFontSize * 2);
            for (var line = 0; line < 12; line++)
            {
                offset += PdfPage.Leading;
                graphics.DrawString(
                    PdfModel.MultiPageLine(pageNumber, line),
                    body,
                    XBrushes.Black,
                    new XPoint(PdfPage.Margin, offset));
            }
        }

        using var output = new MemoryStream();
        document.Save(output, false);
        var bytes = output.ToArray();

        PdfChecks.Document(LibraryCatalog.PdfSharp, bytes, PdfModel.MultiPageCount, PdfModel.MultiPage);
        return bytes;
    }
}
