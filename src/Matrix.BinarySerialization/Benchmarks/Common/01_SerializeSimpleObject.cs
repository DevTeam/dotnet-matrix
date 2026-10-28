// ReSharper disable CheckNamespace
namespace Matrix.BinarySerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SerializeSimpleObject",
    1,
    "Serialize Simple Object",
    "Serializes one scalar object to a binary payload.")]
public partial class SerializeSimpleObject
{
    private readonly SimpleModel _input = SerializationData.Simple();
}
