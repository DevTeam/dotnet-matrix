using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class TemplateRendering
{
    private MicrosoftLoggingFixture _microsoft = null!;
    private ILogger _microsoftLogger = null!;

    [GlobalSetup(Target = nameof(MicrosoftExtensionsLogging))]
    public void SetupMicrosoftExtensionsLogging()
    {
        _microsoft = new MicrosoftLoggingFixture(LogLevel.Information);
        _microsoftLogger = _microsoft.Logger;
    }

    [GlobalCleanup(Target = nameof(MicrosoftExtensionsLogging))]
    public void CleanupMicrosoftExtensionsLogging() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsLogging)]
    public void MicrosoftExtensionsLogging()
    {
        MicrosoftLoggingMessages.Template(
            _microsoftLogger,
            LoggingData.Amount,
            LoggingData.Customer,
            null);
        LoggingChecks.Template(
            LibraryCatalog.MicrosoftExtensionsLogging,
            _microsoft.Sink.Last);
    }
}
