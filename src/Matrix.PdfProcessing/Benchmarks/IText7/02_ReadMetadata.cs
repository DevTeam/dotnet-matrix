using iText.Kernel.Pdf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ReadMetadata
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IText7)]
    public string IText7()
    {
        using var stream = new MemoryStream(PdfFixture.Bytes, false);
        using var document = new PdfDocument(new PdfReader(stream));
        var info = document.GetDocumentInfo();
        var title = info.GetTitle();
        var author = info.GetAuthor();

        PdfChecks.Metadata(LibraryCatalog.IText7, title, author);
        return $"{title}|{author}";
    }
}
