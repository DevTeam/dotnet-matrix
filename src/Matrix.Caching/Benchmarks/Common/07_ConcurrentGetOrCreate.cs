// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ConcurrentGetOrCreate",
    7,
    "Concurrent Get Or Create",
    "Eight threads ask for one absent key at the same time; the value factory must run exactly once.")]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftExtensionsCachingMemory,
    FeatureStatus.Unsupported,
    "Its read-through API runs the value factory once per caller: eight concurrent callers were observed to run it eight times, so the library offers no stampede protection.")]
[FeatureUnavailable(
    LibraryCatalog.SystemRuntimeCaching,
    FeatureStatus.Unsupported,
    "It has no read-through API at all, only a read and a write call.")]
[FeatureUnavailable(
    LibraryCatalog.BitFasterCaching,
    FeatureStatus.Unsupported,
    "Its GetOrAdd runs the value factory once per caller: eight concurrent callers were observed to run it eight times, so the library offers no stampede protection.")]
[FeatureUnavailable(
    LibraryCatalog.EasyCachingInMemory,
    FeatureStatus.Unsupported,
    "Its retriever overload runs the value factory once per caller: eight concurrent callers were observed to run it eight times, so the library offers no stampede protection.")]
[FeatureUnavailable(
    LibraryCatalog.CacheManagerCore,
    FeatureStatus.Unsupported,
    "Its GetOrAdd is not documented as serialising concurrent callers for one key.")]
public partial class ConcurrentGetOrCreate;
