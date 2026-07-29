// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.PerResolve),
    3,
    "PerResolve",
    "Resolves an object graph that asks for the same dependency twice. Both requests inside one resolution share an instance, while the next resolution gets a new one.")]
[FeatureUnavailable(
    LibraryCatalog.Autofac,
    FeatureStatus.Unsupported,
    "Autofac has no native lifetime shared only within one top-level resolve graph.")]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftDi,
    FeatureStatus.Unsupported,
    "Microsoft DI has no native lifetime shared only within one top-level resolve graph.")]
[FeatureUnavailable(
    LibraryCatalog.Lamar,
    FeatureStatus.Unsupported,
    "Lamar has no native lifetime shared only within one top-level resolve graph.")]
[FeatureUnavailable(
    LibraryCatalog.LightInject,
    FeatureStatus.Unsupported,
    "LightInject has no native lifetime shared only within one top-level resolve graph.")]
[FeatureUnavailable(
    LibraryCatalog.Maestro,
    FeatureStatus.Unsupported,
    "Maestro has no native lifetime shared only within one top-level resolve graph.")]
[FeatureUnavailable(
    LibraryCatalog.Ninject,
    FeatureStatus.Unsupported,
    "Ninject provides a call scope only through the separate NamedScope extension, not in the kernel itself.")]
[FeatureUnavailable(
    LibraryCatalog.SimpleInjector,
    FeatureStatus.Unsupported,
    "Simple Injector has no native lifetime shared only within one top-level resolve graph.")]
[FeatureUnavailable(
    LibraryCatalog.StructureMap,
    FeatureStatus.Unsupported,
    "StructureMap has no native lifetime shared only within one top-level resolve graph.")]
[FeatureUnavailable(
    LibraryCatalog.Singularity,
    FeatureStatus.Unsupported,
    "Singularity has no native lifetime shared only within one top-level resolve graph.")]
[FeatureUnavailable(
    LibraryCatalog.ZenIoc,
    FeatureStatus.Unsupported,
    "ZenIoc supports only singleton and transient life cycles.")]
[FeatureUnavailable(
    LibraryCatalog.FasterIoc,
    FeatureStatus.Unsupported,
    "Faster.Ioc supports only singleton, scoped and transient lifetimes.")]
[FeatureUnavailable(
    LibraryCatalog.Catel,
    FeatureStatus.Unsupported,
    "The Catel service locator supports only singleton and transient registrations.")]
[FeatureUnavailable(
    LibraryCatalog.MvvmCross,
    FeatureStatus.Unsupported,
    "The MvvmCross IoC provider supports only singleton and per-resolution-call registrations.")]
[FeatureUnavailable(
    LibraryCatalog.Spring,
    FeatureStatus.Unsupported,
    "Spring.NET object definitions are either singleton or prototype scoped.")]
[FeatureUnavailable(
    LibraryCatalog.Mef2,
    FeatureStatus.Unsupported,
    "MEF sharing is either global or bound to a sharing boundary, never to one resolve graph.")]
[FeatureUnavailable(
    LibraryCatalog.VsMef,
    FeatureStatus.Unsupported,
    "MEF sharing is either global or bound to a sharing boundary, never to one resolve graph.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class PerResolve
{
#if MATRIX_VALIDATION
    private readonly Dictionary<string, IPerResolveDependency> _validationStates = [];
#endif

    [Conditional("MATRIX_VALIDATION")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    private void Validate(string library, PerResolveRoot root)
    {
#if MATRIX_VALIDATION
        MatrixValidation.Same(
            root.First,
            root.Second,
            $"{library} PerResolve dependency differs inside a graph.");
        if (_validationStates.TryGetValue(library, out var previous))
        {
            MatrixValidation.Different(
                root.First,
                previous,
                $"{library} PerResolve dependency leaked between resolves.");
        }

        _validationStates[library] = root.First;
#endif
    }
}
