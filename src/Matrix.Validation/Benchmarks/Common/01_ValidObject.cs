namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "ValidObject",
    1,
    "Valid Object",
    "Validates one object whose scalar properties satisfy every rule.")]
public partial class ValidObject
{
    private readonly BasicInput _input = ValidationData.Valid();
}
