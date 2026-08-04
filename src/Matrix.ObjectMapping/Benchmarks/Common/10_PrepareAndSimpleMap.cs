// ReSharper disable CheckNamespace
namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "PrepareAndSimpleMap",
    10,
    "Prepare And Simple Map",
    "Creates the complete mapper configuration and maps one simple object.")]
public partial class PrepareAndSimpleMap
{
    private readonly SimpleSource _source = ObjectMappingData.Simple();

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, SimpleDestination destination) =>
        ObjectMappingValidation.Simple(library, _source, destination);
}
