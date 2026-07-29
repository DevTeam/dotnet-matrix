namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "CustomRule",
    7,
    "Custom Rule",
    "Applies a custom predicate that accepts only even integer codes.")]
public partial class CustomRule
{
    private readonly CustomInput _input = ValidationData.Custom();
}
