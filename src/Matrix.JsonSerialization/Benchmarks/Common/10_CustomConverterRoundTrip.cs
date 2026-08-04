// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "CustomConverterRoundTrip",
    10,
    "Custom Converter Round Trip",
    "Serializes a strongly typed identifier as a JSON string and deserializes it back.")]
public partial class CustomConverterRoundTrip
{
    private readonly IdentifierModel _input = SerializationData.Identifier();
}
