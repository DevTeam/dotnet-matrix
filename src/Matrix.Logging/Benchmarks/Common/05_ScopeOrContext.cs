// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.ZLogger,
    FeatureStatus.Unsupported,
    "ZLogger providers deliver through a background queue, while this feature requires synchronous sink delivery.")]
[MatrixFeature(
    "ScopeOrContext",
    5,
    "Scope Or Context",
    "Creates a temporary RequestId context and captures it on one event.")]
public partial class ScopeOrContext;
