namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.ZLogger,
    FeatureStatus.Unsupported,
    "ZLogger providers deliver through a background queue, while this feature requires synchronous sink delivery.")]
[MatrixFeature(
    "StructuredProperties",
    3,
    "Structured Properties",
    "Delivers one event with independently queryable OrderId and ElapsedMs properties.")]
public partial class StructuredProperties
{
}
