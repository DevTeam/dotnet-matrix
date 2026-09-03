// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Property),
    7,
    "Property",
    "Resolves three roots that carry writable service properties. The container, or its intended property-injection extension, must assign them during activation.")]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftDi,
    FeatureStatus.Unsupported,
    "Microsoft DI does not provide property injection.")]
[FeatureUnavailable(
    LibraryCatalog.Singularity,
    FeatureStatus.Unsupported,
    "Singularity injects properties only through an explicit LateInject call after activation.")]
[FeatureUnavailable(
    LibraryCatalog.ZenIoc,
    FeatureStatus.Unsupported,
    "ZenIoc does not provide property injection.")]
[FeatureUnavailable(
    LibraryCatalog.FasterIoc,
    FeatureStatus.Unsupported,
    "Faster.Ioc does not provide property injection.")]
[FeatureUnavailable(
    LibraryCatalog.MvvmCross,
    FeatureStatus.Unsupported,
    "MvvmCross property injection only targets interface-typed properties.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Property
{
    [Conditional("MATRIX_VALIDATION")]
    private static void Validate(
        string library,
        IPropertyRoot first,
        IPropertyRoot second,
        IPropertyRoot third)
    {
        Validation.Validation.PropertyRoot(library, first);
        Validation.Validation.PropertyRoot(library, second);
        Validation.Validation.PropertyRoot(library, third);
    }
}
