namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SimpleObject",
    1,
    "Simple Object",
    "Maps one object with scalar values to a newly allocated destination object.")]
public partial class SimpleObject
{
    private readonly SimpleSource _source = ObjectMappingData.Simple();

#if MATRIX_VALIDATION
    private SimpleDestination? _previous;
#endif

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, SimpleDestination destination)
    {
#if MATRIX_VALIDATION
        ObjectMappingValidation.Simple(library, _source, destination, _previous);
        _previous = destination;
#endif
    }
}
