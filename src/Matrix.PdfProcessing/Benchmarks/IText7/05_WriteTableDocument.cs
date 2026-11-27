using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteTableDocument
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
            var table = new Table([1f, 1f, 1f]).UseAllAvailableWidth();
            for (var column = 0; column < PdfModel.TableColumnCount; column++)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(PdfModel.TableHeader(column))
                    .SetFont(font)
                    .SetFontSize((float)PdfPage.BodyFontSize)));
            }

            for (var row = 0; row < PdfModel.TableRowCount; row++)
            {
                for (var column = 0; column < PdfModel.TableColumnCount; column++)
                {
                    table.AddCell(new Cell().Add(new Paragraph(PdfModel.TableCell(row, column))
                        .SetFont(font)
                        .SetFontSize((float)PdfPage.BodyFontSize)));
                }
            }

            layout.Add(table);
            layout.Close();
        }

        var bytes = output.ToArray();

        PdfChecks.Document(LibraryCatalog.IText7, bytes, 1, PdfModel.TableDocument);
        return bytes;
    }
}
