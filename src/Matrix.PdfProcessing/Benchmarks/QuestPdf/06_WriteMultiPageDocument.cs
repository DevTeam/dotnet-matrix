using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteMultiPageDocument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.QuestPdf)]
    public byte[] QuestPdf()
    {
        var document = Document.Create(container =>
        {
            for (var pageNumber = 0; pageNumber < PdfModel.MultiPageCount; pageNumber++)
            {
                var current = pageNumber;
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(72);
                    page.Content().Text(text =>
                    {
                        text.Line(PdfModel.MultiPageHeading(current)).FontSize((float)PdfPage.HeadingFontSize);
                        for (var line = 0; line < 12; line++)
                        {
                            text.Line(PdfModel.MultiPageLine(current, line)).FontSize((float)PdfPage.BodyFontSize);
                        }
                    });
                });
            }
        });
        var bytes = document.GeneratePdf();

        PdfChecks.Document(LibraryCatalog.QuestPdf, bytes, PdfModel.MultiPageCount, PdfModel.MultiPage);
        return bytes;
    }
}
