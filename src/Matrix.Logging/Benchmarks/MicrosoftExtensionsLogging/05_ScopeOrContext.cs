using Microsoft.Extensions.Logging;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class ScopeOrContext
{
    private MicrosoftLoggingFixture _microsoft = null!;
    private ILogger _microsoftLogger = null!;
    private readonly Dictionary<string, object?> _microsoftScope =
        new(StringComparer.Ordinal)
        {
            ["RequestId"] = LoggingData.RequestId
        };

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
        using (_microsoftLogger.BeginScope(_microsoftScope))
        {
            _microsoftLogger.LogInformation(LoggingData.ScopeMessage);
        }

        LoggingChecks.Scope(
            LibraryCatalog.MicrosoftExtensionsLogging,
            _microsoft.Sink.Last);
    }
}
