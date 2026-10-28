// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.CompressionCodecs.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("DecompressIncompressible", 5, "Decompress Incompressible", "Decompresses a pre-made payload whose content is random, so the decoder cannot expand quickly by copying runs.")]
public partial class DecompressIncompressible;
