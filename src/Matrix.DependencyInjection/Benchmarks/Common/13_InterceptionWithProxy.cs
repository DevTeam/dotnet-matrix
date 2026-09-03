// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.InterceptionWithProxy),
    13,
    "Interception With Proxy",
    "Resolves a service through the interception or activation extension point of the library. The result must be a proxy whose interceptor proceeds to the real target.")]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftDi,
    FeatureStatus.Unsupported,
    "Microsoft DI does not provide dynamic interception.")]
[FeatureUnavailable(
    LibraryCatalog.Unity,
    FeatureStatus.Unsupported,
    "Unity.Interception strong-names its generated proxy assembly, which .NET 10 does not support.")]
[FeatureUnavailable(
    LibraryCatalog.Singularity,
    FeatureStatus.Unsupported,
    "Singularity decorators require a statically declared decorator type and cannot return a dynamic proxy.")]
[FeatureUnavailable(
    LibraryCatalog.ZenIoc,
    FeatureStatus.Unsupported,
    "ZenIoc has no interception or activation extension point.")]
[FeatureUnavailable(
    LibraryCatalog.FasterIoc,
    FeatureStatus.Unsupported,
    "Faster.Ioc has no interception or activation extension point.")]
[FeatureUnavailable(
    LibraryCatalog.MvvmCross,
    FeatureStatus.Unsupported,
    "The MvvmCross IoC provider has no interception or activation extension point.")]
[FeatureUnavailable(
    LibraryCatalog.Mef2,
    FeatureStatus.Unsupported,
    "MEF has no interception or activation extension point.")]
[FeatureUnavailable(
    LibraryCatalog.VsMef,
    FeatureStatus.Unsupported,
    "VS MEF has no interception or activation extension point.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class InterceptionWithProxy
{
    [Conditional("MATRIX_VALIDATION")]
    private static void Validate(string library, ICalculator calculator, int value)
    {
        MatrixValidation.Require(library, value == 15, "Intercepted result is invalid.");
        MatrixValidation.Require(library, calculator is not Calculator, "A proxy was not created.");
    }
}
