namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "PrepareConfiguration",
    9,
    "Prepare Configuration",
    "Creates the complete mapper configuration and eagerly prepares its runtime mapping plans.")]
public partial class PrepareConfiguration;
