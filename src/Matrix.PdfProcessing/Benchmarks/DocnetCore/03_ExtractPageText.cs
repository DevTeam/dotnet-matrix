// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ExtractPageText
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DocnetCore)]
    public string DocnetCore()
    {
        using var reader = Docnet.Core.DocLib.Instance.GetDocReader(
            PdfFixture.Bytes,
            new Docnet.Core.Models.PageDimensions(1.0));
        using var page = reader.GetPageReader(0);
        var text = page.GetText();

        PdfChecks.Extracted(LibraryCatalog.DocnetCore, text);
        return text;
    }
}
