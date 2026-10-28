// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "GetHit",
    1,
    "Get Hit",
    "Reads a key that is already present and returns the entry the cache holds.")]
public partial class GetHit;