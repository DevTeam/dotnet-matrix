namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "PrepareValidator",
    10,
    "Prepare Validator",
    "Creates the complete scalar validator or rule graph without validating an input.")]
public partial class PrepareValidator
{
}
