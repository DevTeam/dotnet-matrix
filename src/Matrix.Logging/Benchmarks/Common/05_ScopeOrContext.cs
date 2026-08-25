// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ScopeOrContext",
    5,
    "Scope Or Context",
    "Creates a temporary RequestId context and captures it on one event.")]
public partial class ScopeOrContext;
