// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Conditional),
    11,
    "Conditional",
    "Gives each of three consumers a different implementation of one contract, chosen through the metadata, key, predicate or consumer-context mechanism of the library.")]
[FeatureUnavailable(
    LibraryCatalog.Singularity,
    FeatureStatus.Unsupported,
    "Singularity has no keyed, named, or consumer-conditional registration.")]
[FeatureUnavailable(
    LibraryCatalog.FasterIoc,
    FeatureStatus.Unsupported,
    "Faster.Ioc keyed registrations are reachable only through an explicit keyed resolve and cannot be bound to a consumer constructor.")]
[FeatureUnavailable(
    LibraryCatalog.MvvmCross,
    FeatureStatus.Unsupported,
    "The MvvmCross IoC provider has no keyed, named, or consumer-conditional registration.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Conditional
{
    [Conditional("MATRIX_VALIDATION")]
    private static void Validate(
        string library,
        ConditionalRoot1 first,
        ConditionalRoot2 second,
        ConditionalRoot3 third)
    {
        Validation.Validation.Require(
            library,
            first.Service is ConditionalService1,
            "Conditional binding 1 failed.");
        Validation.Validation.Require(
            library,
            second.Service is ConditionalService2,
            "Conditional binding 2 failed.");
        Validation.Validation.Require(
            library,
            third.Service is ConditionalService3,
            "Conditional binding 3 failed.");
    }
}
