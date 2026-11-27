using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteTableDocument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.QuestPdf)]
    public byte[] QuestPdf()
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(72);
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });
                    table.Header(header =>
                    {
                        for (var column = 0; column < PdfModel.TableColumnCount; column++)
                        {
                            header.Cell().Text(PdfModel.TableHeader(column)).FontSize((float)PdfPage.BodyFontSize);
                        }
                    });
                    for (var row = 0; row < PdfModel.TableRowCount; row++)
                    {
                        for (var column = 0; column < PdfModel.TableColumnCount; column++)
                        {
                            table.Cell().Text(PdfModel.TableCell(row, column)).FontSize((float)PdfPage.BodyFontSize);
                        }
                    }
                });
            });
        });
        var bytes = document.GeneratePdf();

        PdfChecks.Document(LibraryCatalog.QuestPdf, bytes, 1, PdfModel.TableDocument);
        return bytes;
    }
}
