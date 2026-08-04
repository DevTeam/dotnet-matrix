using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class ExceptionLogging
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
        _microsoftLogger.LogError(_exception, LoggingData.ExceptionMessage);
        LoggingChecks.Exception(
            LibraryCatalog.MicrosoftExtensionsLogging,
            _microsoft.Sink.Last,
            _exception);
    }
}
