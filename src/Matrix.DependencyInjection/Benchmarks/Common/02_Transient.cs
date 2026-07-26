// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[FeatureBenchmark(FeatureId.Transient, 2, "Transient")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Transient
{
#if MATRIX_VALIDATION
    private readonly Dictionary<string, ITransient1> _validationStates = [];
#endif

    [Conditional("MATRIX_VALIDATION")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    private void Validate(string library, ITransient1 current)
    {
#if MATRIX_VALIDATION
        if (_validationStates.TryGetValue(library, out var previous))
        {
            Validation.Validation.Different(current, previous, $"{library} transient was reused.");
        }

        _validationStates[library] = current;
#endif
    }
}
