using PdfSharp.Pdf.IO;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ReadMetadata
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfSharp)]
    public string PdfSharp()
    {
        using var stream = new MemoryStream(PdfFixture.Bytes, false);
        using var document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
        var title = document.Info.Title;
        var author = document.Info.Author;

        PdfChecks.Metadata(LibraryCatalog.PdfSharp, title, author);
        return $"{title}|{author}";
    }
}
