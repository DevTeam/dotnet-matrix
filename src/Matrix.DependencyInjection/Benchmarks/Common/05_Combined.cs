// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Combined),
    5,
    "Combined",
    "Resolves three roots that mix singleton and transient dependencies. The singleton is shared across every root while each transient dependency is distinct.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Combined
{
    [Conditional("MATRIX_VALIDATION")]
    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance")]
    private static void Validate(
        string library,
        ICombinedRoot first,
        ICombinedRoot second,
        ICombinedRoot third)
    {
        Validation.Validation.Same(library, first.Singleton, second.Singleton, "Combined singleton differs.");
        Validation.Validation.Same(library, first.Singleton, third.Singleton, "Combined singleton differs.");
        Validation.Validation.Different(library, first.Transient, second.Transient, "Combined transient was reused.");
        Validation.Validation.Different(library, first.Transient, third.Transient, "Combined transient was reused.");
    }
}
