// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.PdfProcessing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ReadPageCount",
    1,
    "Read Page Count",
    "Opens the three-page fixture and reports the page count, with no page content extracted.")]
[FeatureUnavailable(
    LibraryCatalog.QuestPdf,
    FeatureStatus.Unsupported,
    "QuestPDF is a document generator only: it composes and produces documents and exposes no PDF reader, so an existing document cannot be opened.")]
public partial class ReadPageCount : PdfBenchmarkBase;
