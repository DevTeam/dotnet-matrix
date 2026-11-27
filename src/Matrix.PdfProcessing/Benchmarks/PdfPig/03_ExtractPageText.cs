using UglyToad.PdfPig;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ExtractPageText
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfPig)]
    public string PdfPig()
    {
        using var document = PdfDocument.Open(PdfFixture.Bytes);
        var text = document.GetPage(1).Text;

        PdfChecks.Extracted(LibraryCatalog.PdfPig, text);
        return text;
    }
}
