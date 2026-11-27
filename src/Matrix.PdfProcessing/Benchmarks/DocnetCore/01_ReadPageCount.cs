// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ReadPageCount
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DocnetCore)]
    public int DocnetCore()
    {
        using var reader = Docnet.Core.DocLib.Instance.GetDocReader(
            PdfFixture.Bytes,
            new Docnet.Core.Models.PageDimensions(1.0));
        var pages = reader.GetPageCount();

        PdfChecks.PageCount(LibraryCatalog.DocnetCore, pages);
        return pages;
    }
}
