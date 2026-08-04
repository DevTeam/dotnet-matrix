// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "CustomConversion",
    7,
    "Custom Conversion",
    "Maps string values through registered code and invariant decimal conversions.")]
public partial class CustomConversion
{
    private readonly ConversionSource _source = ObjectMappingData.Conversion();

#if MATRIX_VALIDATION
    private ConversionDestination? _previous;
#endif

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, ConversionDestination destination)
    {
#if MATRIX_VALIDATION
        ObjectMappingValidation.Conversion(library, destination, _previous);
        _previous = destination;
#endif
    }
}
