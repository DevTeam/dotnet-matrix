// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Enumerable),
    9,
    "IEnumerable",
    "Injects a sequence of five plugin implementations and requires it to be genuinely lazy: nothing is created until enumeration, and every enumeration yields new transients.",
    rated: false,
    reason: "Too few rated libraries implement genuine lazy enumeration for this to be a competitive result; the scenario was measured and drawn in no chart group before it briefly entered one by accident.")]
[FeatureUnavailable(
    LibraryCatalog.Autofac,
    FeatureStatus.Unsupported,
    "Autofac materializes IEnumerable<T> before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.Grace,
    FeatureStatus.Unsupported,
    "Grace materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.Lamar,
    FeatureStatus.Unsupported,
    "Lamar materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.LightInject,
    FeatureStatus.Unsupported,
    "LightInject materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftDi,
    FeatureStatus.Unsupported,
    "Microsoft DI materializes IEnumerable<T> before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.Maestro,
    FeatureStatus.Unsupported,
    "Maestro materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.Ninject,
    FeatureStatus.Unsupported,
    "Ninject materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.Windsor,
    FeatureStatus.Unsupported,
    "Castle Windsor materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.Unity,
    FeatureStatus.Unsupported,
    "Unity materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.StructureMap,
    FeatureStatus.Unsupported,
    "StructureMap materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.Stashbox,
    FeatureStatus.Unsupported,
    "Stashbox materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.Singularity,
    FeatureStatus.Unsupported,
    "Singularity materializes IEnumerable<T> into an array before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.ZenIoc,
    FeatureStatus.Unsupported,
    "ZenIoc does not resolve collections of registered implementations.")]
[FeatureUnavailable(
    LibraryCatalog.FasterIoc,
    FeatureStatus.Unsupported,
    "Faster.Ioc does not resolve collections of registered implementations.")]
[FeatureUnavailable(
    LibraryCatalog.MvvmCross,
    FeatureStatus.Unsupported,
    "The MvvmCross IoC provider does not resolve collections of registered implementations.")]
[FeatureUnavailable(
    LibraryCatalog.Spring,
    FeatureStatus.Unsupported,
    "Spring.NET resolves a managed list eagerly before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.Mef2,
    FeatureStatus.Unsupported,
    "MEF materializes an ImportMany collection before activating the consumer.")]
[FeatureUnavailable(
    LibraryCatalog.VsMef,
    FeatureStatus.Unsupported,
    "VS MEF materializes an ImportMany collection before activating the consumer.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Enumerable
{
    [Conditional("MATRIX_VALIDATION")]
    private static void Validate(
        string library,
        IEnumerableRoot first,
        IEnumerableRoot second,
        IEnumerableRoot third) =>
        Validation.Validation.EnumerableRoots(library, first, second, third);
}
