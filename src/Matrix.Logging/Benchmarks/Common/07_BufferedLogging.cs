// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftExtensionsLogging,
    FeatureStatus.Unsupported,
    "Microsoft.Extensions.Logging defines no asynchronous or buffering provider in the core package.")]
[MatrixFeature(
    "BufferedLogging",
    7,
    "Buffered Logging",
    "Enqueues one event to a library-provided async or buffering wrapper and validates delivery after flush.")]
public partial class BufferedLogging;

