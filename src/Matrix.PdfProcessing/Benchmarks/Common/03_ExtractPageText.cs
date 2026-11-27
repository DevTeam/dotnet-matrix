// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ExtractPageText",
    3,
    "Extract Page Text",
    "Opens the fixture and extracts the text of the first page, including content stream decoding.")]
[FeatureUnavailable(
    LibraryCatalog.PdfSharp,
    FeatureStatus.Unsupported,
    "PDFsharp 6 has no text extraction API; its XTextFormatter only lays text out when writing, so extraction would be an adapter implementing the parser itself.")]
[FeatureUnavailable(
    LibraryCatalog.PdfSharpCore,
    FeatureStatus.Unsupported,
    "PdfSharpCore has no text extraction API; its XTextFormatter only lays text out when writing, so extraction would be an adapter implementing the parser itself.")]
[FeatureUnavailable(
    LibraryCatalog.QuestPdf,
    FeatureStatus.Unsupported,
    "QuestPDF is a document generator only: it composes content when producing a document and exposes no reader or text extractor.")]
public partial class ExtractPageText : PdfBenchmarkBase;
