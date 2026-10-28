// ReSharper disable CheckNamespace
namespace Matrix.BinarySerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "EnumRoundTrip",
    7,
    "Enum Round Trip",
    "Round-trips one enum member through the library's binary representation.")]
public partial class EnumRoundTrip
{
    private readonly EnumModel _input = SerializationData.Enum();
}
