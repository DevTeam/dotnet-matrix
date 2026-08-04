// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftExtensionsLogging,
    FeatureStatus.Unsupported,
    "Microsoft.Extensions.Logging defines no output formatter in the core package.")]
[FeatureUnavailable(
    LibraryCatalog.OpenTelemetry,
    FeatureStatus.Unsupported,
    "OpenTelemetry exports log records but does not define a comparable text formatter.")]
[MatrixFeature(
    "FormattedOutput",
    9,
    "Formatted Output",
    "Formats one event synchronously into a bounded in-memory sink.")]
public partial class FormattedOutput
{
    private static void Verify(string library, FormattedOutputSink sink) =>
        LoggingChecks.FormattedOutput(library, sink.Count, sink.Last);
}
