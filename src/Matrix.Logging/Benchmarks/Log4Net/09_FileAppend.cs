using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Matrix.Logging.Benchmarks;

public partial class FileAppend
{
    private string _log4NetPath = null!;
    private string _log4NetRepository = null!;
    private FileAppender _log4NetAppender = null!;
    private ILog _log4NetLogger = null!;

    [GlobalSetup(Target = nameof(Log4Net))]
    public void SetupLog4Net()
    {
        _log4NetPath = CreateFilePath(LibraryCatalog.Log4Net);
        _log4NetRepository = $"{LoggingData.Category}.{Guid.NewGuid():N}";
        var repository = (Hierarchy)LogManager.CreateRepository(_log4NetRepository);
        _log4NetAppender = new FileAppender
        {
            File = _log4NetPath,
            AppendToFile = true,
            ImmediateFlush = false,
            Layout = new PatternLayout(LoggingData.Log4NetFileLayout)
        };
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
        Verify(LibraryCatalog.Log4Net, _log4NetPath);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Log4Net)]
    public void Log4Net() => _log4NetLogger.Info(LoggingData.FileMessage);
}
