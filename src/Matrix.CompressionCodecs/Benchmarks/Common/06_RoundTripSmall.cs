// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.CompressionCodecs.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("RoundTripSmall", 6, "Round Trip Small", "Compresses and decompresses a 1 KiB payload, the shape most individual messages take.")]
public partial class RoundTripSmall;
