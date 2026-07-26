// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[FeatureBenchmark(
    FeatureId.PrepareAndRegisterAndSimpleResolve,
    15,
    "Prepare And Register And Simple Resolve")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class PrepareAndRegisterAndSimpleResolve;
