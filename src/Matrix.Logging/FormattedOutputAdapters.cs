using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using NLog;
using NLog.Targets;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using ZLogger;

namespace Matrix.Logging;

internal sealed class Log4NetFormattedOutputAppender(
    FormattedOutputSink sink,
    ILayout layout) : AppenderSkeleton
{
    protected override void Append(LoggingEvent loggingEvent)
    {
        sink.BeginRecord();
        layout.Format(sink, loggingEvent);
        sink.EndTextRecord();
    }
}

internal sealed class NLogFormattedOutputTarget(
    FormattedOutputSink sink) : TargetWithLayout
{
    protected override void Write(LogEventInfo logEvent)
    {
        sink.BeginRecord();
        sink.Write(RenderLogEvent(Layout, logEvent));
        sink.EndTextRecord();
    }
}

internal sealed class SerilogFormattedOutputSink(
    FormattedOutputSink sink,
    ITextFormatter formatter) : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        sink.BeginRecord();
        formatter.Format(logEvent, sink);
        sink.EndTextRecord();
    }
}

internal sealed class ZLoggerFormattedOutputProcessor(
    FormattedOutputSink sink,
    IZLoggerFormatter formatter) : IAsyncLogProcessor
{
    public void Post(IZLoggerEntry log)
    {
        try
        {
            sink.BeginRecord();
            log.FormatUtf8(sink, formatter);
            sink.EndUtf8Record();
        }
        finally
        {
            log.Return();
        }
    }

    public ValueTask DisposeAsync() => default;
}
