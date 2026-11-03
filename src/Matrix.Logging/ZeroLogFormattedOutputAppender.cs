using ZeroLog.Formatting;

namespace Matrix.Logging;

/// <summary>
/// Routes a ZeroLog event through one of the library's own pattern-based formatters and writes
/// the exact rendered characters into the shared bounded text sink, so the measured call pays for
/// rendering rather than for the matrix.
/// </summary>
/// <remarks>
/// <see cref="DefaultFormatter.FormatMessage"/> appends the record's line terminator, which is
/// more than the contract asks for, so the pattern writer is used directly: it fills a caller
/// buffer with the pattern text and reports how many characters it produced.
/// </remarks>
internal sealed class ZeroLogFormattedOutputAppender(
    FormattedOutputSink sink,
    PatternWriter patternWriter) : ZeroLog.Appenders.Appender
{
    private readonly char[] _buffer = new char[512];

    public override void WriteMessage(LoggedMessage message)
    {
        patternWriter.Write(message, _buffer, out var written);
        sink.BeginRecord();
        sink.Write(_buffer.AsSpan(0, written));
        sink.EndTextRecord();
    }
}
