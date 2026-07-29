// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Generics),
    8,
    "Generics",
    "Registers one open generic service mapping and resolves roots closed over int, float and object. Registering every closed type separately does not count.")]
[FeatureUnavailable(
    LibraryCatalog.ZenIoc,
    FeatureStatus.Unsupported,
    "ZenIoc cannot register an open generic service mapping.")]
[FeatureUnavailable(
    LibraryCatalog.Catel,
    FeatureStatus.Unsupported,
    "The Catel service locator cannot close an open generic registration.")]
[FeatureUnavailable(
    LibraryCatalog.MvvmCross,
    FeatureStatus.Unsupported,
    "The MvvmCross IoC provider cannot close an open generic registration.")]
[FeatureUnavailable(
    LibraryCatalog.Spring,
    FeatureStatus.Unsupported,
    "Spring.NET object definitions cannot declare an open generic type.")]
[FeatureUnavailable(
    LibraryCatalog.VsMef,
    FeatureStatus.Unsupported,
    "VS MEF part discovery rejects open generic types.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Generics;
