// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.PrepareAndRegisterAndSimpleResolve),
    15,
    "Prepare And Register And Simple Resolve",
    "Measures the same setup as Prepare And Register, followed by a single resolve of one singleton root.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class PrepareAndRegisterAndSimpleResolve;
