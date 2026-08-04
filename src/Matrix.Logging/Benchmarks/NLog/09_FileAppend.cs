using NLog;
using NLog.Config;
using NLog.Targets;

namespace Matrix.Logging.Benchmarks;

public partial class FileAppend
{
    private string _nlogPath = null!;
    private LogFactory _nlogFactory = null!;
    private Logger _nlogLogger = null!;

    [GlobalSetup(Target = nameof(NLog))]
    public void SetupNLog()
    {
        _nlogPath = CreateFilePath(LibraryCatalog.NLog);
        var target = new FileTarget
        {
            FileName = _nlogPath,
            KeepFileOpen = true,
            AutoFlush = false,
            Layout = LoggingData.NLogFileLayout
        };
        var configuration = new LoggingConfiguration();
        configuration.AddRule(LogLevel.Info, LogLevel.Fatal, target, LoggingData.Category);
        _nlogFactory = new LogFactory { Configuration = configuration };
        _nlogLogger = _nlogFactory.GetLogger(LoggingData.Category);
    }

    [GlobalCleanup(Target = nameof(NLog))]
    public void CleanupNLog()
    {
        _nlogFactory.Flush(TimeSpan.FromSeconds(5));
        _nlogFactory.Shutdown();
        _nlogFactory.Dispose();
        Verify(LibraryCatalog.NLog, _nlogPath);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NLog)]
    public void NLog() => _nlogLogger.Info(LoggingData.FileMessage);
}
