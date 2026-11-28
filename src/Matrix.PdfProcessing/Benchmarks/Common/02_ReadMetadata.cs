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
public partial class ReadMetadata : PdfBenchmarkBase;
