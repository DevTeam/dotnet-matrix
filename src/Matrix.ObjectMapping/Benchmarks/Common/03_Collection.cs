namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Collection",
    3,
    "Collection",
    "Maps an array of 100 objects while preserving count, order and member values.")]
public partial class Collection
{
    private readonly SimpleSource[] _source = ObjectMappingData.Collection();

#if MATRIX_VALIDATION
    private SimpleDestination[]? _previous;
#endif

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, SimpleDestination[] destination)
    {
#if MATRIX_VALIDATION
        ObjectMappingValidation.Collection(library, _source, destination, _previous);
        _previous = destination;
#endif
    }
}
