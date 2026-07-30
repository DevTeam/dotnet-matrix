using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class StructuredProperties
{
    private MicrosoftLoggingFixture _microsoft = null!;

    [GlobalSetup(Target = nameof(MicrosoftExtensionsLogging))]
    public void SetupMicrosoftExtensionsLogging() =>
        _microsoft = new MicrosoftLoggingFixture(LogLevel.Information);

    [GlobalCleanup(Target = nameof(MicrosoftExtensionsLogging))]
    public void CleanupMicrosoftExtensionsLogging() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsLogging)]
    public void MicrosoftExtensionsLogging()
    {
        _microsoft.Logger.LogInformation(
            LoggingData.StructuredTemplate,
            LoggingData.OrderId,
            LoggingData.ElapsedMs);
        LoggingChecks.Structured(
            LibraryCatalog.MicrosoftExtensionsLogging,
            _microsoft.Sink.Last);
    }
}

