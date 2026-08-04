// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "EnumRoundTrip",
    9,
    "Enum Round Trip",
    "Serializes an enum as its string name and deserializes it back.")]
public partial class EnumRoundTrip
{
    private readonly EnumModel _input = SerializationData.Enum();
}
