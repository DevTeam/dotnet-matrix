using NLog;
using NLog.Config;

namespace Matrix.Logging.Benchmarks;

public partial class FormattedOutput
{
    private FormattedOutputSink _nlogSink = null!;
    private LogFactory _nlogFactory = null!;
    private Logger _nlogLogger = null!;

    [GlobalSetup(Target = nameof(NLog))]
    public void SetupNLog()
    {
        _nlogSink = new();
        var target = new NLogFormattedOutputTarget(_nlogSink)
        {
            Layout = LoggingData.NLogOutputLayout
        };
        var configuration = new LoggingConfiguration();
        configuration.AddRule(LogLevel.Info, LogLevel.Fatal, target, LoggingData.Category);
        _nlogFactory = new LogFactory { Configuration = configuration };
        _nlogLogger = _nlogFactory.GetLogger(LoggingData.Category);
    }

    [GlobalCleanup(Target = nameof(NLog))]
    public void CleanupNLog()
    {
        _nlogFactory.Shutdown();
        _nlogFactory.Dispose();
        Verify(LibraryCatalog.NLog, _nlogSink);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    public void NLog() => _nlogLogger.Info(LoggingData.OutputMessage);
}
