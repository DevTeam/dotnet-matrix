namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.ServiceStackText,
    FeatureStatus.Unsupported,
    "ServiceStack.Text cannot reproduce the approved safe cat/dog discriminator contract without unrestricted runtime type loading.")]
[MatrixFeature(
    "PolymorphicRoundTrip",
    11,
    "Polymorphic Round Trip",
    "Round-trips a base-type collection through safe cat and dog discriminators.")]
public partial class PolymorphicRoundTrip
{
    private readonly ZooModel _input = SerializationData.Zoo();
}
