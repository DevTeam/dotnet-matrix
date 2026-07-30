namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.ZLogger,
    FeatureStatus.Unsupported,
    "ZLogger providers deliver through a background queue, while this feature requires synchronous sink delivery.")]
[MatrixFeature(
    "ExceptionLogging",
    4,
    "Exception",
    "Delivers one Error event retaining the original exception metadata.")]
public partial class ExceptionLogging
{
    private readonly InvalidOperationException _exception =
        new("Database unavailable.");
}
