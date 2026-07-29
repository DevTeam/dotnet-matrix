// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Scoped),
    4,
    "Scoped",
    "Resolves scoped services inside explicit scopes. One instance per scope, different instances across scopes, and scope-owned disposables are disposed when the scope ends.")]
[FeatureUnavailable(
    LibraryCatalog.ZenIoc,
    FeatureStatus.Unsupported,
    "ZenIoc has no explicit resolution scope.")]
[FeatureUnavailable(
    LibraryCatalog.Catel,
    FeatureStatus.Unsupported,
    "The Catel service locator has no explicit resolution scope.")]
[FeatureUnavailable(
    LibraryCatalog.MvvmCross,
    FeatureStatus.Unsupported,
    "The MvvmCross IoC provider has no explicit resolution scope.")]
[FeatureUnavailable(
    LibraryCatalog.Spring,
    FeatureStatus.Unsupported,
    "Spring.NET core object factories have no explicit resolution scope.")]
[FeatureUnavailable(
    LibraryCatalog.Mef2,
    FeatureStatus.Unsupported,
    "MEF sharing boundaries are entered only through ExportFactory<T> and cannot be resolved from repeatedly.")]
[FeatureUnavailable(
    LibraryCatalog.VsMef,
    FeatureStatus.Unsupported,
    "MEF sharing boundaries are entered only through ExportFactory<T> and cannot be resolved from repeatedly.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Scoped
{
#if MATRIX_VALIDATION
    private readonly Dictionary<string, IScopedDependency> _validationStates = [];
#endif

    [Conditional("MATRIX_VALIDATION")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    private void Validate(string library, ScopedRoot first, ScopedRoot second)
    {
#if MATRIX_VALIDATION
        Validation.Validation.Same(
            first.Dependency,
            second.Dependency,
            $"{library} scoped dependency differs inside a scope.");
        if (_validationStates.TryGetValue(library, out var previous))
        {
            Validation.Validation.Different(
                first.Dependency,
                previous,
                $"{library} scoped dependency leaked between scopes.");
        }

        _validationStates[library] = first.Dependency;
#endif
    }
}
