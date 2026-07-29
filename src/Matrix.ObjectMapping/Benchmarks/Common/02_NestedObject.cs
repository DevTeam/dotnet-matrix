namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "NestedObject",
    2,
    "Nested Object",
    "Maps an order with nested customer and address objects to a new destination graph.")]
public partial class NestedObject
{
    private readonly OrderSource _source = ObjectMappingData.Order();

#if MATRIX_VALIDATION
    private OrderDestination? _previous;
#endif

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, OrderDestination destination)
    {
#if MATRIX_VALIDATION
        ObjectMappingValidation.Nested(library, _source, destination, _previous);
        _previous = destination;
#endif
    }
}
