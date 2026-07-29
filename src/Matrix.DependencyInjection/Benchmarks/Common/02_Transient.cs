// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Transient),
    2,
    "Transient",
    "Registers three transient services and resolves each of them repeatedly. Every resolve must create a new instance, never reusing an earlier one.")]
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
            MatrixValidation.Different(current, previous, $"{library} transient was reused.");
        }

        _validationStates[library] = current;
#endif
    }
}
