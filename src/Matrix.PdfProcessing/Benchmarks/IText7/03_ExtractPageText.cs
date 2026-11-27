using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ExtractPageText
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IText7)]
    public string IText7()
    {
        using var stream = new MemoryStream(PdfFixture.Bytes, false);
        using var document = new PdfDocument(new PdfReader(stream));
        var text = PdfTextExtractor.GetTextFromPage(document.GetPage(1));

        PdfChecks.Extracted(LibraryCatalog.IText7, text);
        return text;
    }
}
