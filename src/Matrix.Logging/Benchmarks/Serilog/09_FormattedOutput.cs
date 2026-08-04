using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Matrix.Logging.Benchmarks;

public partial class FormattedOutput
{
    private FormattedOutputSink _serilogOutput = null!;
    private Serilog.Core.Logger _serilogRoot = null!;
    private Serilog.ILogger _serilogLogger = null!;

    [GlobalSetup(Target = nameof(Serilog))]
    public void SetupSerilog()
    {
        _serilogOutput = new();
        var sink = new SerilogFormattedOutputSink(
            _serilogOutput,
            new MessageTemplateTextFormatter(LoggingData.SerilogOutputTemplate));
        _serilogRoot = new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Information)
            .WriteTo.Sink(sink)
            .CreateLogger();
        _serilogLogger = _serilogRoot.ForContext("SourceContext", LoggingData.Category);
    }

    [GlobalCleanup(Target = nameof(Serilog))]
    public void CleanupSerilog()
    {
        _serilogRoot.Dispose();
        Verify(LibraryCatalog.Serilog, _serilogOutput);
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Serilog)]
    public void Serilog() => _serilogLogger.Information(LoggingData.OutputMessage);
}
