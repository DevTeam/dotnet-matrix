using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Benchmarks;

public partial class ScopeOrContext
{
    private MicrosoftLoggingFixture _microsoft = null!;
    private readonly Dictionary<string, object?> _microsoftScope =
        new(StringComparer.Ordinal)
        {
            ["RequestId"] = LoggingData.RequestId
        };

    [GlobalSetup(Target = nameof(MicrosoftExtensionsLogging))]
    public void SetupMicrosoftExtensionsLogging() =>
        _microsoft = new MicrosoftLoggingFixture(LogLevel.Information);

    [GlobalCleanup(Target = nameof(MicrosoftExtensionsLogging))]
    public void CleanupMicrosoftExtensionsLogging() => _microsoft.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsLogging)]
    public void MicrosoftExtensionsLogging()
    {
        using (_microsoft.Logger.BeginScope(_microsoftScope))
        {
            _microsoft.Logger.LogInformation(LoggingData.ScopeMessage);
        }

        LoggingChecks.Scope(
            LibraryCatalog.MicrosoftExtensionsLogging,
            _microsoft.Sink.Last);
    }
}

