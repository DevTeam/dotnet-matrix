namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "DeserializeCollection",
    6,
    "Deserialize Collection",
    "Deserializes a compact JSON array to three ordered objects.")]
public partial class DeserializeCollection
{
    private const string Input = SerializationData.CollectionJson;
}
