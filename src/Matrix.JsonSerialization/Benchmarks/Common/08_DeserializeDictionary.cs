// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "DeserializeDictionary",
    8,
    "Deserialize Dictionary",
    "Deserializes a JSON object to three ordinal string and integer entries.")]
public partial class DeserializeDictionary
{
    private const string Input = SerializationData.DictionaryJson;
}
