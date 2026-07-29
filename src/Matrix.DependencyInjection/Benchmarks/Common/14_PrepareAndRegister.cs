// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.PrepareAndRegister),
    14,
    "Prepare And Register",
    "Measures creating the container and registering the whole prescribed graph, without resolving anything from it.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class PrepareAndRegister;
