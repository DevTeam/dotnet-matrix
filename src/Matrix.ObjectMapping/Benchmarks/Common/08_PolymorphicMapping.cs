namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "PolymorphicMapping",
    8,
    "Polymorphic Mapping",
    "Maps a base array containing cats and dogs to matching destination runtime types.")]
public partial class PolymorphicMapping
{
    private readonly AnimalSource[] _source = ObjectMappingData.Animals();

#if MATRIX_VALIDATION
    private AnimalDestination[]? _previous;
#endif

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, AnimalDestination[] destination)
    {
#if MATRIX_VALIDATION
        ObjectMappingValidation.Polymorphic(library, _source, destination, _previous);
        _previous = destination;
#endif
    }
}
