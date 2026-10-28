// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.CompressionCodecs.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("CompressText", 1, "Compress Text", "Compresses the deterministic 256 KiB repetitive text corpus at the codec's own fastest level.")]
public partial class CompressText;
