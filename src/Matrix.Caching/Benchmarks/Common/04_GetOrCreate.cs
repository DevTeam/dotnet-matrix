// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "GetOrCreate",
    4,
    "Get Or Create",
    "Reads a present key through the read-through API, so the value factory must not run.")]
[FeatureUnavailable(
    LibraryCatalog.EasyCachingInMemory,
    FeatureStatus.Unsupported,
    "Its retriever overload takes a Func<CacheEntry> without the key and is documented for a single caller; the scenario needs a keyed read-through that reports the stored entry.")]
[FeatureUnavailable(
    LibraryCatalog.CacheManagerCore,
    FeatureStatus.Unsupported,
    "Its GetOrAdd overloads take a value or a factory of the key only and return the stored item through a separate API; the scenario needs a read-through call that returns the stored entry for a present key.")]
[FeatureUnavailable(
    LibraryCatalog.BitFasterCaching,
    FeatureStatus.Unsupported,
    "Its GetOrAdd returns the value created by the factory argument overload but the cached entry is keyed by int only; the string-keyed scenario cannot be expressed.")]
public partial class GetOrCreate;
