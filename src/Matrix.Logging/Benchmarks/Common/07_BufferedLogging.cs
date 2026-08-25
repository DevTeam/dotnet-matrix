// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftExtensionsLogging,
    FeatureStatus.Unsupported,
    "Microsoft.Extensions.Logging defines no asynchronous or buffering provider in the core package.")]
[FeatureUnavailable(
    LibraryCatalog.ZLogger,
    FeatureStatus.Unsupported,
    "ZLogger's processor API delivers synchronously; only its built-in stream, file, and console providers offer a genuine bounded background buffer, and this scenario's in-memory sink does not use one of those.")]
[MatrixFeature(
    "BufferedLogging",
    7,
    "Buffered Logging",
    "Enqueues one event to a library-provided async or buffering wrapper and validates delivery after flush. Measures the cost of accepting the event; the remaining delivery work happens on a background thread.")]
public partial class BufferedLogging;

