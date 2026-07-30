namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SerializeDictionary",
    7,
    "Serialize Dictionary",
    "Serializes three ordered string and integer entries to a JSON object.")]
public partial class SerializeDictionary
{
    private readonly Dictionary<string, int> _input = SerializationData.Dictionary();
}
