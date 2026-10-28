// ReSharper disable CheckNamespace

namespace Matrix.BinarySerialization.Benchmarks;



[MemoryDiagnoser]

[Orderer(SummaryOrderPolicy.FastestToSlowest)]

[FeatureUnavailable(

    LibraryCatalog.HyperSerializer,

    FeatureStatus.Unsupported,

    "HyperSerializer generates no code for collection-typed members: an array property is skipped and the payload comes out empty, so the collection cannot be written or read back.")]

[MatrixFeature(

    "SerializeCollection",

    5,

    "Serialize Collection",

    "Serializes three ordered objects to a binary payload.")]

public partial class SerializeCollection

{

    private readonly CollectionModel _input = SerializationData.Collection();

}

