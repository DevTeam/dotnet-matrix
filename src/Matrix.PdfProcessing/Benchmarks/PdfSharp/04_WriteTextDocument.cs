using PdfSharp.Drawing;
using PdfSharp.Pdf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteTextDocument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfSharp)]
    public byte[] PdfSharp()
    {
        PdfChecks.FontAvailable(LibraryCatalog.PdfSharp);
        using var document = new PdfDocument();
        var page = document.AddPage();
        page.Width = XUnit.FromPoint(PdfPage.Width);
        page.Height = XUnit.FromPoint(PdfPage.Height);
        using (var graphics = XGraphics.FromPdfPage(page))
        {
            var heading = new XFont(PdfSystemFontResolver.Family, PdfPage.HeadingFontSize);
            graphics.DrawString(PdfModel.Heading, heading, XBrushes.Black, new XPoint(PdfPage.Margin, PdfPage.Margin + PdfPage.HeadingFontSize));
            var body = new XFont(PdfSystemFontResolver.Family, PdfPage.BodyFontSize);
            var offset = PdfPage.Margin + (PdfPage.HeadingFontSize * 2);
            foreach (var line in PdfModel.TextDocument)
            {
                if (string.Equals(line, PdfModel.Heading, StringComparison.Ordinal))
                {
                    continue;
                }

                offset += PdfPage.Leading;
                graphics.DrawString(line, body, XBrushes.Black, new XPoint(PdfPage.Margin, offset));
            }
        }

        using var output = new MemoryStream();
        document.Save(output, false);
        var bytes = output.ToArray();

        PdfChecks.Document(LibraryCatalog.PdfSharp, bytes, 1, PdfModel.TextDocument);
        return bytes;
    }
}
