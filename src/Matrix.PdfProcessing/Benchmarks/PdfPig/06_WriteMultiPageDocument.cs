using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class WriteMultiPageDocument
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfPig)]
    public byte[] PdfPig()
    {
        var builder = new PdfDocumentBuilder();
        var font = builder.AddStandard14Font(Standard14Font.Helvetica);
        for (var pageNumber = 0; pageNumber < PdfModel.MultiPageCount; pageNumber++)
        {
            var page = builder.AddPage(PdfPage.Width, PdfPage.Height);
            page.AddText(
                PdfModel.MultiPageHeading(pageNumber),
                PdfPage.HeadingFontSize,
                new PdfPoint(PdfPage.Margin, PdfPage.Height - PdfPage.Margin),
                font);
            var offset = PdfPage.Height - PdfPage.Margin - (PdfPage.HeadingFontSize * 2);
            for (var line = 0; line < 12; line++)
            {
                offset -= PdfPage.Leading;
                page.AddText(
                    PdfModel.MultiPageLine(pageNumber, line),
                    PdfPage.BodyFontSize,
                    new PdfPoint(PdfPage.Margin, offset),
                    font);
            }
        }

        var bytes = builder.Build();

        PdfChecks.Document(LibraryCatalog.PdfPig, bytes, PdfModel.MultiPageCount, PdfModel.MultiPage);
        return bytes;
    }
}
