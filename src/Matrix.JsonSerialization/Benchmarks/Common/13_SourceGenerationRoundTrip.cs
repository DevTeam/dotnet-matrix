// ReSharper disable CheckNamespace
namespace Matrix.JsonSerialization.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.NewtonsoftJson,
    FeatureStatus.NotApplicable,
    "Newtonsoft.Json has no source-generation programming model.")]
[FeatureUnavailable(
    LibraryCatalog.ServiceStackText,
    FeatureStatus.NotApplicable,
    "ServiceStack.Text has no source-generation programming model.")]
[MatrixFeature(
    "SourceGenerationRoundTrip",
    13,
    "Source Generation Round Trip",
    "Round-trips a simple object with compile-time generated JSON metadata.",
    rated: false,
    reason: "With this few rated entrants, the reference is a library's own result, not a result earned against a competitor, so the full 200 points would not reflect a win.")]
public partial class SourceGenerationRoundTrip
{
    private readonly SimpleModel _input = SerializationData.Simple();
}
