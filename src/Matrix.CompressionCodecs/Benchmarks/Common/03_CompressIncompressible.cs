// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.CompressionCodecs.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("CompressIncompressible", 3, "Compress Incompressible", "Compresses the deterministic 256 KiB random corpus, where no codec can win by finding redundancy.")]
public partial class CompressIncompressible;
