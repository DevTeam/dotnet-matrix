using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
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
        MicrosoftLoggingMessages.Template(
            _microsoft.Logger,
            LoggingData.Amount,
            LoggingData.Customer,
            null);
        LoggingChecks.Template(
            LibraryCatalog.MicrosoftExtensionsLogging,
            _microsoft.Sink.Last);
    }
}

