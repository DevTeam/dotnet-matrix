namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Flattening",
    4,
    "Flattening",
    "Maps nested customer values into a flat order summary through member-path configuration.")]
public partial class Flattening
{
    private readonly OrderSource _source = ObjectMappingData.Order();

#if MATRIX_VALIDATION
    private OrderSummaryDestination? _previous;
#endif

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, OrderSummaryDestination destination)
    {
#if MATRIX_VALIDATION
        ObjectMappingValidation.Flattening(library, _source, destination, _previous);
        _previous = destination;
#endif
    }
}
