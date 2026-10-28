// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "GetMiss",
    2,
    "Get Miss",
    "Reads a key that is absent and reports the absence without allocating an entry.")]
[FeatureUnavailable(
    LibraryCatalog.CacheManagerCore,
    FeatureStatus.Unsupported,
    "CacheManager.Core returns default(T) for a missing key, so a cache miss cannot be told apart from a stored default value.")]
public partial class GetMiss;