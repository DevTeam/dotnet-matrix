namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "NullHandling",
    6,
    "Null Handling",
    "Preserves null text, nested object and collection members in the destination.")]
public partial class NullHandling
{
    private readonly NullableSource _source = ObjectMappingData.Nullable();

#if MATRIX_VALIDATION
    private NullableDestination? _previous;
#endif

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, NullableDestination destination)
    {
#if MATRIX_VALIDATION
        ObjectMappingValidation.NullHandling(library, destination, _previous);
        _previous = destination;
#endif
    }
}
