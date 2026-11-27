// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "WriteTableDocument",
    5,
    "Write Table Document",
    "Produces a one-page document holding a thirty-row, three-column table with a header row, and saves it to a byte array.")]
[FeatureUnavailable(
    LibraryCatalog.DocnetCore,
    FeatureStatus.Unsupported,
    "Docnet.Core wraps the PDFium reader only: it exposes document readers, merging, splitting and rendering, but no document authoring API.")]
[FeatureUnavailable(
    LibraryCatalog.PdfSharpCore,
    FeatureStatus.Unsupported,
    "PdfSharpCore is compared as a reader only; its writer is the PDFsharp 1.x code it was forked from, so measuring it against PDFsharp 6 would compare a package with its own ancestor.")]
public partial class WriteTableDocument : PdfBenchmarkBase;
