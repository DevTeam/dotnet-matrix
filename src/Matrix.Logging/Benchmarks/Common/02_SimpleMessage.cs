// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.ZLogger,
    FeatureStatus.Unsupported,
    "ZLogger providers deliver through a background queue, while this feature requires synchronous sink delivery.")]
[MatrixFeature(
    "SimpleMessage",
    2,
    "Simple Message",
    "Delivers one literal Information message to an in-memory sink.")]
public partial class SimpleMessage;
