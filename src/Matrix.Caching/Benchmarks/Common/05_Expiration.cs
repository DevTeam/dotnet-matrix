// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Expiration",
    5,
    "Expiration",
    "Stores an entry with a fifty-millisecond lifetime, waits for it to elapse, then reads it.")]
[FeatureUnavailable(
    LibraryCatalog.BitFasterCaching,
    FeatureStatus.Unsupported,
    "Its unbounded LRU policy has no per-entry lifetime, so an entry cannot be made to expire.")]
public partial class Expiration;
