// ReSharper disable CheckNamespace
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
[FeatureUnavailable(
    LibraryCatalog.MicrosoftExtensionsValidation,
    FeatureStatus.Unsupported,
    "Microsoft.Extensions.Validation 10 does not support asynchronous validation rules.")]
[MatrixFeature(
    "AsyncValidation",
    9,
    "Async Validation",
    "Runs a deterministic asynchronous availability rule through the library async API.",
    rated: false,
    reason: "With this few rated entrants, the reference is a library's own result, not a result earned against a competitor, so the full 200 points would not reflect a win.")]
public partial class AsyncValidation
{
    private readonly AsyncInput _input = ValidationData.Async();
}
