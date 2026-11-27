using iText.Kernel.Pdf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ReadPageCount
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IText7)]
    public int IText7()
    {
        using var stream = new MemoryStream(PdfFixture.Bytes, false);
        using var document = new PdfDocument(new PdfReader(stream));
        var pages = document.GetNumberOfPages();

        PdfChecks.PageCount(LibraryCatalog.IText7, pages);
        return pages;
    }
}
