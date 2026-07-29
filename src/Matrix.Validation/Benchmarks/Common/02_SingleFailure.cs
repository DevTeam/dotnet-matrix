namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SingleFailure",
    2,
    "Single Failure",
    "Validates one object and returns its single property failure.")]
public partial class SingleFailure
{
    private readonly BasicInput _input = ValidationData.SingleFailure();
}
