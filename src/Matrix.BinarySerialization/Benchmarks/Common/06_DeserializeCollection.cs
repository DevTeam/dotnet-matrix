// ReSharper disable CheckNamespace

namespace Matrix.BinarySerialization.Benchmarks;



[MemoryDiagnoser]

[Orderer(SummaryOrderPolicy.FastestToSlowest)]

[FeatureUnavailable(
    LibraryCatalog.HyperSerializer,
    FeatureStatus.Unsupported,
    "HyperSerializer generates no code for collection-typed members: an array property is skipped and the payload comes out empty, so the collection cannot be written or read back.")]
[MatrixFeature(

    "DeserializeCollection",

    6,

    "Deserialize Collection",

    "Deserializes three ordered objects from a binary payload.")]

public partial class DeserializeCollection

{

    private byte[] _payload = [];



}

