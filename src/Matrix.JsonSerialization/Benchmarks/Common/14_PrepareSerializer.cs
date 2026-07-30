namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "PrepareSerializer",
    14,
    "Prepare Serializer",
    "Creates fresh serializer settings and explicit type metadata without serializing data.")]
public partial class PrepareSerializer
{
}
