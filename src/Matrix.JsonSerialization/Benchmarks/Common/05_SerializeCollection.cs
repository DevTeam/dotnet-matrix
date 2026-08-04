// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SerializeCollection",
    5,
    "Serialize Collection",
    "Serializes three ordered objects to a compact JSON array.")]
public partial class SerializeCollection
{
    private readonly SimpleModel[] _input = SerializationData.Collection();
}
