using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class DisabledLog
{
    private MicrosoftLoggingFixture _microsoft = null!;
    private ILogger _microsoftLogger = null!;

    [GlobalSetup(Target = nameof(MicrosoftExtensionsLogging))]
    public void SetupMicrosoftExtensionsLogging()
    {
        _microsoft = new MicrosoftLoggingFixture(LogLevel.Warning);
        _microsoftLogger = _microsoft.Logger;
    }

    [GlobalCleanup(Target = nameof(MicrosoftExtensionsLogging))]
    public void CleanupMicrosoftExtensionsLogging() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsLogging)]
    public void MicrosoftExtensionsLogging()
    {
        _microsoftLogger.LogInformation(LoggingData.SuppressedMessage);
        LoggingChecks.Disabled(
            LibraryCatalog.MicrosoftExtensionsLogging,
            _microsoft.Sink.Count);
    }
}
