// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.CompressionCodecs.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("CompressTextMax", 2, "Compress Text Max Effort", "Compresses the same 256 KiB text corpus at the codec's own maximum general-purpose level, showing what the extra effort costs.")]
public partial class CompressTextMax;
