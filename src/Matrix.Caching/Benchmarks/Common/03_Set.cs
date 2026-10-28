// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Set",
    3,
    "Set",
    "Stores one entry under a fresh key with a ten-minute lifetime.")]
public partial class Set;