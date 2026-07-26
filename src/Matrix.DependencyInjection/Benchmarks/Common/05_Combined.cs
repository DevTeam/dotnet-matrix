// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[FeatureBenchmark(FeatureId.Combined, 5, "Combined")]
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
