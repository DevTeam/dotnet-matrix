using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Matrix.Logging;

internal sealed class Log4NetFixture : IDisposable
{
    private readonly BufferingForwardingAppender? _buffer;

    public Log4NetFixture(Level minimumLevel, bool buffered = false)
    {
        var repositoryName = $"{LoggingData.Category}.{Guid.NewGuid():N}";
        Repository = (Hierarchy)LogManager.CreateRepository(repositoryName);
        Sink = new Log4NetCaptureAppender();
        Sink.ActivateOptions();
        IAppender appender = Sink;
        if (buffered)
        {
            _buffer = new BufferingForwardingAppender
            {
                BufferSize = LoggingData.BufferedCapacity,
                Fix = FixFlags.Partial,
                Lossy = false
            };
            _buffer.AddAppender(Sink);
            _buffer.ActivateOptions();
            appender = _buffer;
        }

        BasicConfigurator.Configure(Repository, appender);
        Repository.Root.Level = minimumLevel;
        Repository.Configured = true;
        Logger = LogManager.GetLogger(repositoryName, LoggingData.Category);
    }

    public Log4NetCaptureAppender Sink { get; }

    public ILog Logger { get; }

    private Hierarchy Repository { get; }

    public void Flush() => _buffer?.Flush();

    public void Dispose()
    {
        _buffer?.Close();
        LogManager.ShutdownRepository(Repository.Name);
    }
}
