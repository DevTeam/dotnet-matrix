// ReSharper disable CheckNamespace
namespace Matrix.BinarySerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.HyperSerializer,
    FeatureStatus.Unsupported,
    "HyperSerializer serializes only scalar members: it skips a property whose type is a class, so the nested customer and address cannot be written or read back.")]
[MatrixFeature(
    "SerializeNestedObject",
    3,
    "Serialize Nested Object",
    "Serializes an order with a nested customer and address to a binary payload.")]
public partial class SerializeNestedObject
{
    private readonly OrderModel _input = SerializationData.Nested();
}
