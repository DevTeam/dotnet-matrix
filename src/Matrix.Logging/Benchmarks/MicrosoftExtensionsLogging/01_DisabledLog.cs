using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class DisabledLog
{
    private MicrosoftLoggingFixture _microsoft = null!;

    [GlobalSetup(Target = nameof(MicrosoftExtensionsLogging))]
    public void SetupMicrosoftExtensionsLogging() =>
        _microsoft = new MicrosoftLoggingFixture(LogLevel.Warning);

    [GlobalCleanup(Target = nameof(MicrosoftExtensionsLogging))]
    public void CleanupMicrosoftExtensionsLogging() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsLogging)]
    public void MicrosoftExtensionsLogging()
    {
        _microsoft.Logger.LogInformation(LoggingData.SuppressedMessage);
        LoggingChecks.Disabled(
            LibraryCatalog.MicrosoftExtensionsLogging,
            _microsoft.Sink.Count);
    }
}

