using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteTextDocument
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
                page.Content().Text(text =>
                {
                    text.Line(PdfModel.Heading).FontSize((float)PdfPage.HeadingFontSize);
                    foreach (var line in PdfModel.TextDocument)
                    {
                        if (!string.Equals(line, PdfModel.Heading, StringComparison.Ordinal))
                        {
                            text.Line(line).FontSize((float)PdfPage.BodyFontSize);
                        }
                    }
                });
            });
        });
        var bytes = document.GeneratePdf();

        PdfChecks.Document(LibraryCatalog.QuestPdf, bytes, 1, PdfModel.TextDocument);
        return bytes;
    }
}
