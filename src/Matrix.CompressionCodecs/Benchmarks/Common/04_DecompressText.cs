// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.CompressionCodecs.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("DecompressText", 4, "Decompress Text", "Decompresses a payload produced beforehand, so only the decoder is measured.")]
public partial class DecompressText;
