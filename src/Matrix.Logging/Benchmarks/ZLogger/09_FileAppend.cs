using Microsoft.Extensions.Logging;
using ZLogger;

namespace Matrix.Logging.Benchmarks;

public partial class FileAppend
{
    private string _zloggerPath = null!;
    private ILoggerFactory _zloggerFactory = null!;
    private ILogger _zloggerLogger = null!;

    [GlobalSetup(Target = nameof(ZLogger))]
    public void SetupZLogger()
    {
        _zloggerPath = CreateFilePath(LibraryCatalog.ZLogger);
        _zloggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddZLoggerFile(_zloggerPath, options => options.UsePlainTextFormatter(formatter =>
                formatter.SetPrefixFormatter(
                    $"{0:yyyy-MM-dd HH:mm:ss,fff} {1:short} {2} - ",
                    (in MessageTemplate template, in LogInfo info) =>
                        template.Format(info.Timestamp.Local.DateTime, info.LogLevel, info.Category))));
        });
        _zloggerLogger = _zloggerFactory.CreateLogger(LoggingData.Category);
    }

    [GlobalCleanup(Target = nameof(ZLogger))]
    public void CleanupZLogger()
    {
        // Disposing the factory drains the background queue and closes the file.
        _zloggerFactory.Dispose();
        Verify(LibraryCatalog.ZLogger, _zloggerPath);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.ZLogger)]
    public void ZLogger() => _zloggerLogger.LogInformation(LoggingData.FileMessage);
}
