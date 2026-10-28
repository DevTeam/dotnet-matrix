// ReSharper disable CheckNamespace

namespace Matrix.BinarySerialization.Benchmarks;



[MemoryDiagnoser]

[Orderer(SummaryOrderPolicy.FastestToSlowest)]

[FeatureUnavailable(

    LibraryCatalog.HyperSerializer,

    FeatureStatus.Unsupported,

    "HyperSerializer serializes only scalar members: it skips a property whose type is a class, so the nested customer and address cannot be written or read back.")]

[MatrixFeature(

    "DeserializeNestedObject",

    4,

    "Deserialize Nested Object",

    "Deserializes an order with a nested customer and address from a binary payload.")]

public partial class DeserializeNestedObject

{
    private byte[] _payload = [];


}

