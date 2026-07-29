namespace Matrix.Validation.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.DataAnnotations,
    FeatureStatus.Unsupported,
    "DataAnnotations exposes only synchronous validation APIs.")]
[FeatureUnavailable(
    LibraryCatalog.MiniValidation,
    FeatureStatus.Unsupported,
    "MiniValidation exposes only synchronous validation APIs.")]
[MatrixFeature(
    "AsyncValidation",
    9,
    "Async Validation",
    "Runs a deterministic asynchronous availability rule through the library async API.")]
public partial class AsyncValidation
{
    private readonly AsyncInput _input = ValidationData.Async();
}
