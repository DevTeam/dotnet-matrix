// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "SerializeNestedObject",
    3,
    "Serialize Nested Object",
    "Serializes an order, customer, and address object graph.")]
public partial class SerializeNestedObject
{
    private readonly OrderModel _input = SerializationData.Nested();
}
