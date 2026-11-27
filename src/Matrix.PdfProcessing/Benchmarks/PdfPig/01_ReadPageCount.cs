using UglyToad.PdfPig;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ReadPageCount
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfPig)]
    public int PdfPig()
    {
        using var document = PdfDocument.Open(PdfFixture.Bytes);
        var pages = document.NumberOfPages;

        PdfChecks.PageCount(LibraryCatalog.PdfPig, pages);
        return pages;
    }
}
