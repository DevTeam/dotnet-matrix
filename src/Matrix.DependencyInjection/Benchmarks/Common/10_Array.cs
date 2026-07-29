// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Array),
    10,
    "Array",
    "Resolves three roots that materialise their injected sequence of five plugins into an array while the root is being activated.")]
[FeatureUnavailable(
    LibraryCatalog.ZenIoc,
    FeatureStatus.Unsupported,
    "ZenIoc does not resolve collections of registered implementations.")]
[FeatureUnavailable(
    LibraryCatalog.FasterIoc,
    FeatureStatus.Unsupported,
    "Faster.Ioc does not resolve collections of registered implementations.")]
[FeatureUnavailable(
    LibraryCatalog.Catel,
    FeatureStatus.Unsupported,
    "Catel resolves collections only through an explicit ResolveTypes call, never as an injected dependency.")]
[FeatureUnavailable(
    LibraryCatalog.MvvmCross,
    FeatureStatus.Unsupported,
    "The MvvmCross IoC provider does not resolve collections of registered implementations.")]
[FeatureUnavailable(
    LibraryCatalog.Maestro,
    FeatureStatus.Unsupported,
    "Maestro does not resolve an array of registered implementations as an injected dependency.")]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftDi,
    FeatureStatus.Unsupported,
    "Microsoft.Extensions.DependencyInjection resolves multiple registrations as IEnumerable<T>, not as T[].")]
[FeatureUnavailable(
    LibraryCatalog.SimpleInjector,
    FeatureStatus.Unsupported,
    "Simple Injector injects registered collections as IEnumerable<T>, not as T[].")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Array
{
    [Conditional("MATRIX_VALIDATION")]
    private static void Validate(
        string library,
        ArrayRoot1 first,
        ArrayRoot2 second,
        ArrayRoot3 third) =>
        Validation.Validation.ArrayRoots(library, first, second, third);
}
