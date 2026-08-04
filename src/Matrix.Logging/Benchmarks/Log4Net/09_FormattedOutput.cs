using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Matrix.Logging.Benchmarks;

public partial class FormattedOutput
{
    private string _log4NetRepository = null!;
    private FormattedOutputSink _log4NetSink = null!;
    private Log4NetFormattedOutputAppender _log4NetAppender = null!;
    private ILog _log4NetLogger = null!;

    [GlobalSetup(Target = nameof(Log4Net))]
    public void SetupLog4Net()
    {
        _log4NetRepository = $"{LoggingData.Category}.{Guid.NewGuid():N}";
        var repository = (Hierarchy)LogManager.CreateRepository(_log4NetRepository);
        _log4NetSink = new();
        _log4NetAppender = new(
            _log4NetSink,
            new PatternLayout(LoggingData.Log4NetOutputLayout));
        _log4NetAppender.ActivateOptions();
        BasicConfigurator.Configure(repository, _log4NetAppender);
        repository.Root.Level = Level.Info;
        repository.Configured = true;
        _log4NetLogger = LogManager.GetLogger(_log4NetRepository, LoggingData.Category);
    }

    [GlobalCleanup(Target = nameof(Log4Net))]
    public void CleanupLog4Net()
    {
        _log4NetAppender.Close();
        LogManager.ShutdownRepository(_log4NetRepository);
        Verify(LibraryCatalog.Log4Net, _log4NetSink);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    public void Log4Net() => _log4NetLogger.Info(LoggingData.OutputMessage);
}
