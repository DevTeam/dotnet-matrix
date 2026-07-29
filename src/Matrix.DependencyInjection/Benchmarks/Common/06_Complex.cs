// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Complex),
    6,
    "Complex",
    "Registers and resolves three multi-level object graphs, checking that every nested dependency has the expected implementation type and lifetime.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Complex;
