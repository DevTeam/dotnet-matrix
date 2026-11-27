using UglyToad.PdfPig;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

public partial class ReadMetadata
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.PdfPig)]
    public string PdfPig()
    {
        using var document = PdfDocument.Open(PdfFixture.Bytes);
        var title = document.Information.Title;
        var author = document.Information.Author;

        PdfChecks.Metadata(LibraryCatalog.PdfPig, title, author);
        return $"{title}|{author}";
    }
}
