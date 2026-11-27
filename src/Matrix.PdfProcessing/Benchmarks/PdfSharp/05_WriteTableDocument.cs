using PdfSharp.Drawing;
using PdfSharp.Pdf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteTableDocument
{
    /// <summary>
    /// PDFsharp 6 has no table abstraction, so the adapter positions the cells and
    /// draws the grid itself. That is exactly the arithmetic the scenario
    /// description attributes to a library without a table layout engine.
    /// </summary>
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
            var font = new XFont(PdfSystemFontResolver.Family, PdfPage.BodyFontSize);
            var offset = PdfPage.Margin;
            graphics.DrawString(
                string.Join(" | ", Enumerable.Range(0, PdfModel.TableColumnCount).Select(PdfModel.TableHeader)),
                font,
                XBrushes.Black,
                new XPoint(PdfPage.Margin, offset));
            offset += PdfPage.Leading;
            for (var row = 0; row < PdfModel.TableRowCount; row++)
            {
                offset += PdfPage.TableRowHeight;
                var cells = new string[PdfModel.TableColumnCount];
                for (var column = 0; column < PdfModel.TableColumnCount; column++)
                {
                    cells[column] = PdfModel.TableCell(row, column);
                }

                graphics.DrawString(string.Join(" | ", cells), font, XBrushes.Black, new XPoint(PdfPage.Margin, offset));
            }
        }

        using var output = new MemoryStream();
        document.Save(output, false);
        var bytes = output.ToArray();

        PdfChecks.Document(LibraryCatalog.PdfSharp, bytes, 1, PdfModel.TableDocument);
        return bytes;
    }
}
