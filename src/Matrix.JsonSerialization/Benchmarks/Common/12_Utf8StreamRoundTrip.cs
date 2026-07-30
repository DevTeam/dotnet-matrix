namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Utf8StreamRoundTrip",
    12,
    "UTF-8 Stream Round Trip",
    "Serializes a simple object to a new UTF-8 memory stream and deserializes it back.")]
public partial class Utf8StreamRoundTrip
{
    private readonly SimpleModel _input = SerializationData.Simple();
}
