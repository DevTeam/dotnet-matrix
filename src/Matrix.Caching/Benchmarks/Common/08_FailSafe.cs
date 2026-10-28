// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "FailSafe",
    8,
    "Fail Safe",
    "Serves the last known value when the value factory throws after the entry expired.")]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftExtensionsCachingMemory,
    FeatureStatus.Unsupported,
    "It has no fail-safe concept: a throwing value factory propagates the exception.")]
[FeatureUnavailable(
    LibraryCatalog.SystemRuntimeCaching,
    FeatureStatus.Unsupported,
    "It has no read-through API and therefore no fail-safe fallback.")]
[FeatureUnavailable(
    LibraryCatalog.LazyCache,
    FeatureStatus.Unsupported,
    "It has no fail-safe concept: a throwing factory propagates the exception.")]
[FeatureUnavailable(
    LibraryCatalog.BitFasterCaching,
    FeatureStatus.Unsupported,
    "It has no fail-safe concept: a throwing factory propagates the exception.")]
[FeatureUnavailable(
    LibraryCatalog.EasyCachingInMemory,
    FeatureStatus.Unsupported,
    "It has no fail-safe concept: a throwing retriever propagates the exception.")]
[FeatureUnavailable(
    LibraryCatalog.CacheManagerCore,
    FeatureStatus.Unsupported,
    "It has no fail-safe concept: a throwing factory propagates the exception.")]
public partial class FailSafe;
