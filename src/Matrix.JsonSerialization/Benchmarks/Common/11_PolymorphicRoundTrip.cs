// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.ServiceStackText,
    FeatureStatus.Unsupported,
    "ServiceStack.Text cannot reproduce the approved safe cat/dog discriminator contract without unrestricted runtime type loading.")]
[FeatureUnavailable(
    LibraryCatalog.SpanJson,
    FeatureStatus.Unsupported,
    "SpanJson has no discriminator or derived-type mechanism for an abstract base type: it serializes the runtime members only, and deserializing the base type fails on its unknown discriminator member.")]
[MatrixFeature(
    "PolymorphicRoundTrip",
    11,
    "Polymorphic Round Trip",
    "Round-trips a base-type collection through safe cat and dog discriminators.")]
public partial class PolymorphicRoundTrip
{
    private readonly ZooModel _input = SerializationData.Zoo();
}
