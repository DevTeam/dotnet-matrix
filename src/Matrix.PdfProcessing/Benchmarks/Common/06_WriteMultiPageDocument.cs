// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "WriteMultiPageDocument",
    6,
    "Write Multi Page Document",
    "Produces a four-page document whose pages each carry a heading and twelve lines, and saves it to a byte array.")]
[FeatureUnavailable(
    LibraryCatalog.DocnetCore,
    FeatureStatus.Unsupported,
    "Docnet.Core wraps the PDFium reader only: it exposes document readers, merging, splitting and rendering, but no document authoring API.")]
[FeatureUnavailable(
    LibraryCatalog.PdfSharpCore,
    FeatureStatus.Unsupported,
    "PdfSharpCore is compared as a reader only; its writer is the PDFsharp 1.x code it was forked from, so measuring it against PDFsharp 6 would compare a package with its own ancestor.")]
public partial class WriteMultiPageDocument : PdfBenchmarkBase;
