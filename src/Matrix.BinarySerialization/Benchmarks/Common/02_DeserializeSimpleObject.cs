// ReSharper disable CheckNamespace
namespace Matrix.BinarySerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "DeserializeSimpleObject",
    2,
    "Deserialize Simple Object",
    "Deserializes one scalar object from a binary payload produced by the same library.")]
public partial class DeserializeSimpleObject
{
    private byte[] _payload = [];

}
