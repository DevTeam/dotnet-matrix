// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "DeserializeSimpleObject",
    2,
    "Deserialize Simple Object",
    "Deserializes one compact JSON object and validates every scalar member.")]
public partial class DeserializeSimpleObject
{
    private const string Input = SerializationData.SimpleJson;
}
