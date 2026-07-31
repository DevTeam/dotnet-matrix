namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.MiniValidation,
    FeatureStatus.Unsupported,
    "MiniValidation has no fail-fast or cascade mode and always collects the object errors.")]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftExtensionsValidation,
    FeatureStatus.Unsupported,
    "Microsoft.Extensions.Validation has no fail-fast or cascade mode and validates the complete object.")]
[MatrixFeature(
    "StopOnFirstFailure",
    8,
    "Stop On First Failure",
    "Stops validation after the first failing rule in the declared order.")]
public partial class StopOnFirstFailure
{
    private readonly BasicInput _input = ValidationData.MultipleFailures();
}
