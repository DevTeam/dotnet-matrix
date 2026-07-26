// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[FeatureBenchmark(FeatureId.ChildContainer, 12, "Child Container")]
[FeatureUnavailable(
    LibraryCatalog.Grace,
    FeatureStatus.Unsupported,
    "Grace child scopes cannot override a dependency of a registration owned by the parent scope.")]
[FeatureUnavailable(
    LibraryCatalog.Lamar,
    FeatureStatus.Unsupported,
    "Lamar nested containers cannot override a dependency of a registration owned by the root container.")]
[FeatureUnavailable(
    LibraryCatalog.LightInject,
    FeatureStatus.Unsupported,
    "LightInject provides scopes only and has no nested container with its own registrations.")]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftDi,
    FeatureStatus.Unsupported,
    "Microsoft DI scopes cannot add or override registrations.")]
[FeatureUnavailable(
    LibraryCatalog.PureDi,
    FeatureStatus.Unsupported,
    "Pure.DI generated scopes cannot add runtime registrations.")]
[FeatureUnavailable(
    LibraryCatalog.Windsor,
    FeatureStatus.Unsupported,
    "Castle Windsor child containers cannot override a dependency of a component owned by the parent.")]
[FeatureUnavailable(
    LibraryCatalog.Maestro,
    FeatureStatus.Unsupported,
    "Maestro scopes cannot add or override registrations.")]
[FeatureUnavailable(
    LibraryCatalog.Ninject,
    FeatureStatus.Unsupported,
    "A Ninject child kernel cannot supply dependencies to objects created by the parent kernel.")]
[FeatureUnavailable(
    LibraryCatalog.SimpleInjector,
    FeatureStatus.Unsupported,
    "Simple Injector does not support child containers with registration overrides.")]
[FeatureUnavailable(
    LibraryCatalog.Singularity,
    FeatureStatus.Unsupported,
    "Singularity nested containers cannot override a registration that already exists in the parent graph.")]
[FeatureUnavailable(
    LibraryCatalog.ZenIoc,
    FeatureStatus.Unsupported,
    "A ZenIoc child container cannot override a dependency of a registration owned by the parent.")]
[FeatureUnavailable(
    LibraryCatalog.FasterIoc,
    FeatureStatus.Unsupported,
    "A Faster.Ioc child container cannot override a dependency of a registration owned by the parent.")]
[FeatureUnavailable(
    LibraryCatalog.MvvmCross,
    FeatureStatus.Unsupported,
    "An MvvmCross child container cannot override a dependency of a registration owned by the parent.")]
[FeatureUnavailable(
    LibraryCatalog.Spring,
    FeatureStatus.Unsupported,
    "A Spring.NET child object factory cannot override a dependency of a definition owned by the parent.")]
[FeatureUnavailable(
    LibraryCatalog.Mef2,
    FeatureStatus.Unsupported,
    "A MEF composition host is immutable and has no nested host with its own parts.")]
[FeatureUnavailable(
    LibraryCatalog.VsMef,
    FeatureStatus.Unsupported,
    "A VS MEF export provider is built from an immutable catalog and has no nested provider with its own parts.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class ChildContainer
{
    [Conditional("MATRIX_VALIDATION")]
    private static void Validate(string library, ChildRoot parent, ChildRoot child)
    {
        Validation.Validation.Require(
            parent.Value is ParentValue,
            $"{library} child registration leaked into the parent container.");
        Validation.Validation.Require(
            child.Value is ChildValue,
            $"{library} child registration was not used.");
    }
}
