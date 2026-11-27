// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ReadMetadata",
    2,
    "Read Metadata",
    "Opens the fixture and reads the document title and author from the information dictionary.")]
[FeatureUnavailable(
    LibraryCatalog.DocnetCore,
    FeatureStatus.Unsupported,
    "Docnet.Core exposes the PDFium text layer, page count and rendering, but no document information dictionary, so a title and author cannot be read through it.")]
[FeatureUnavailable(
    LibraryCatalog.QuestPdf,
    FeatureStatus.Unsupported,
    "QuestPDF is a document generator only: it accepts metadata when producing a document and exposes no reader, so an existing document's dictionary cannot be read.")]
public partial class ReadMetadata : PdfBenchmarkBase;
