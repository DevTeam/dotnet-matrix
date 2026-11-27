using PdfSharpCore.Pdf.IO;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ReadMetadata
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfSharpCore)]
    public string PdfSharpCore()
    {
        using var stream = new MemoryStream(PdfFixture.Bytes, false);
        using var document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
        var title = document.Info.Title;
        var author = document.Info.Author;

        PdfChecks.Metadata(LibraryCatalog.PdfSharpCore, title, author);
        return $"{title}|{author}";
    }
}
