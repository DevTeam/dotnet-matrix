namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "MultipleFailures",
    3,
    "Multiple Failures",
    "Validates one object and materializes three independent property failures.")]
public partial class MultipleFailures
{
    private readonly BasicInput _input = ValidationData.MultipleFailures();
}
