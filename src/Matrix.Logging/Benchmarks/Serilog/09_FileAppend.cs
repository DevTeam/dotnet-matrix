using Serilog;
using Serilog.Events;

namespace Matrix.Logging.Benchmarks;

public partial class FileAppend
{
    private string _serilogPath = null!;
    private Serilog.Core.Logger _serilogRoot = null!;
    private Serilog.ILogger _serilogLogger = null!;

    [GlobalSetup(Target = nameof(Serilog))]
    public void SetupSerilog()
    {
        _serilogPath = CreateFilePath(LibraryCatalog.Serilog);
        _serilogRoot = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Information)
            .WriteTo.File(
                _serilogPath,
                buffered: true,
                outputTemplate: LoggingData.SerilogFileTemplate)
            .CreateLogger();
        _serilogLogger = _serilogRoot.ForContext("SourceContext", LoggingData.Category);
    }

    [GlobalCleanup(Target = nameof(Serilog))]
    public void CleanupSerilog()
    {
        _serilogRoot.Dispose();
        Verify(LibraryCatalog.Serilog, _serilogPath);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Serilog)]
    public void Serilog() => _serilogLogger.Information(LoggingData.FileMessage);
}
