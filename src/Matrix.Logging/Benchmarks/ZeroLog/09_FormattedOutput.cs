using ZeroLog;
using ZeroLog.Appenders;
using ZeroLog.Configuration;
using ZeroLog.Formatting;
// ReSharper disable CheckNamespace
namespace Matrix.Logging.Benchmarks;

public partial class FormattedOutput
{
    private FormattedOutputSink _zeroLogOutput = null!;
    private Log _zeroLogLogger = null!;
    private IDisposable _zeroLogSession = default!;

    [GlobalSetup(Target = nameof(ZeroLog))]
    public void SetupZeroLog()
    {
        _zeroLogOutput = new FormattedOutputSink();
        var appender = new ZeroLogFormattedOutputAppender(
            _zeroLogOutput,
            new PatternWriter(LoggingData.ZeroLogOutputLayout));
        var configuration = new ZeroLogConfiguration
        {
            AutoRegisterEnums = false,
            AppendingStrategy = AppendingStrategy.Synchronous
        };
        configuration.RootLogger.Level = LogLevel.Info;
        configuration.RootLogger.Appenders.Add(appender);
        _zeroLogSession = LogManager.Initialize(configuration);
        _zeroLogLogger = LogManager.GetLogger(LoggingData.Category);
    }

    [GlobalCleanup(Target = nameof(ZeroLog))]
    public void CleanupZeroLog()
    {
        _zeroLogSession.Dispose();
        LogManager.Shutdown();
        Verify(LibraryCatalog.ZeroLog, _zeroLogOutput);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZeroLog)]
    public void ZeroLog() => _zeroLogLogger.Info(LoggingData.OutputMessage);
}
