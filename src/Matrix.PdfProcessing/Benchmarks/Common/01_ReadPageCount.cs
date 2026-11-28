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
public partial class ReadPageCount : PdfBenchmarkBase;
