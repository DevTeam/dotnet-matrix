// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.ZeroLog,
    FeatureStatus.Unsupported,
    "ZeroLog has no ambient scope or context stack; it attaches named values only to the event being built.")]
[MatrixFeature(
    "ScopeOrContext",
    5,
    "Scope Or Context",
    "Creates a temporary RequestId context and captures it on one event.")]
public partial class ScopeOrContext;
