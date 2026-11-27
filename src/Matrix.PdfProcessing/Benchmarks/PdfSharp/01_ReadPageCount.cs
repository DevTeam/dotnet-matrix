using PdfSharp.Pdf.IO;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ReadPageCount
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfSharp)]
    public int PdfSharp()
    {
        using var stream = new MemoryStream(PdfFixture.Bytes, false);
        using var document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
        var pages = document.PageCount;

        PdfChecks.PageCount(LibraryCatalog.PdfSharp, pages);
        return pages;
    }
}
