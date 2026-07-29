namespace Matrix.ObjectMapping.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "MapToExisting",
    5,
    "Map To Existing",
    "Overwrites a supplied destination object and returns that same instance.")]
public partial class MapToExisting
{
    private readonly SimpleSource _source = ObjectMappingData.Simple();
    private readonly SimpleDestination _destination = new()
    {
        Id = -1,
        Name = "Before",
        Amount = -1,
        CreatedAt = DateTime.UnixEpoch,
        Active = false
    };

    [Conditional("MATRIX_VALIDATION")]
    private void Validate(string library, SimpleDestination destination) =>
        ObjectMappingValidation.Existing(library, _source, _destination, destination);
}
