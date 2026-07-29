// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.DependencyInjection.Benchmarks;

[MatrixFeature(
    nameof(FeatureId.Singleton),
    1,
    "Singleton",
    "Registers three singleton services and resolves each of them repeatedly. Every resolve of the same service must return the same instance.")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public partial class Singleton
{
#if MATRIX_VALIDATION
    private readonly Dictionary<string, ValidationState> _validationStates = [];
#endif

    [Conditional("MATRIX_VALIDATION")]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    private void Validate(
        string library,
        ISingleton1 first,
        ISingleton2 second,
        ISingleton3 third)
    {
#if MATRIX_VALIDATION
        if (_validationStates.TryGetValue(library, out var previous))
        {
            Validation.Validation.Same(first, previous.First, $"{library} Singleton1 changed.");
            Validation.Validation.Same(second, previous.Second, $"{library} Singleton2 changed.");
            Validation.Validation.Same(third, previous.Third, $"{library} Singleton3 changed.");
        }

        _validationStates[library] = new ValidationState(first, second, third);
#endif
    }

#if MATRIX_VALIDATION
    private sealed record ValidationState(
        ISingleton1 First,
        ISingleton2 Second,
        ISingleton3 Third);
#endif
}
