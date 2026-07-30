namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SerializeSimpleObject",
    1,
    "Serialize Simple Object",
    "Serializes one scalar object to a compact JSON string.")]
public partial class SerializeSimpleObject
{
    private readonly SimpleModel _input = SerializationData.Simple();
}
