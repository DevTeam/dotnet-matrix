// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Invalidate",
    6,
    "Invalidate",
    "Removes a present key and confirms the entry is gone.")]
public partial class Invalidate;