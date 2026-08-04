// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "DeserializeNestedObject",
    4,
    "Deserialize Nested Object",
    "Deserializes and materializes an order, customer, and address object graph.")]
public partial class DeserializeNestedObject
{
    private const string Input = SerializationData.NestedJson;
}
