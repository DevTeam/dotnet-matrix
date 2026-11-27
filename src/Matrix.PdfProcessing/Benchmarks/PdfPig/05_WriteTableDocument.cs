using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteTableDocument
{
    /// <summary>
    /// Like PDFsharp, PdfPig has no table abstraction, so the adapter positions the
    /// cells itself — the arithmetic the scenario attributes to that kind of library.
    /// </summary>
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfPig)]
    public byte[] PdfPig()
    {
        var builder = new PdfDocumentBuilder();
        var font = builder.AddStandard14Font(Standard14Font.Helvetica);
        var page = builder.AddPage(PdfPage.Width, PdfPage.Height);
        var offset = PdfPage.Height - PdfPage.Margin;
        page.AddText(
            string.Join(" | ", Enumerable.Range(0, PdfModel.TableColumnCount).Select(PdfModel.TableHeader)),
            PdfPage.BodyFontSize,
            new PdfPoint(PdfPage.Margin, offset),
            font);
        offset -= PdfPage.Leading;
        for (var row = 0; row < PdfModel.TableRowCount; row++)
        {
            var cells = new string[PdfModel.TableColumnCount];
            for (var column = 0; column < PdfModel.TableColumnCount; column++)
            {
                cells[column] = PdfModel.TableCell(row, column);
            }

            offset -= PdfPage.TableRowHeight;
            page.AddText(string.Join(" | ", cells), PdfPage.BodyFontSize, new PdfPoint(PdfPage.Margin, offset), font);
        }

        var bytes = builder.Build();

        PdfChecks.Document(LibraryCatalog.PdfPig, bytes, 1, PdfModel.TableDocument);
        return bytes;
    }
}
