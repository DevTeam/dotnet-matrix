using System.Text;

namespace Matrix.Logging;

/// <summary>
/// Write destination for <c>AddZLoggerStream</c>, the only ZLogger integration
/// point that dispatches through a genuine background buffer
/// (<c>ZLoggerOptions.BackgroundBufferCapacity</c>/<c>FullMode</c>); the direct
/// <c>IAsyncLogProcessor</c> overload used by <see cref="ZLoggerCaptureProcessor"/>
/// always runs on the calling thread regardless of those options. Multiple
/// buffered records can arrive in a single <see cref="Write"/> call once the
/// background writer drains, so records are split on newlines rather than
/// counted per call.
/// </summary>
internal sealed class ZLoggerBufferedSink : Stream
{
#if MATRIX_VALIDATION
    private int _count;
    private string _pending = "";
#endif

    public int Count =>
#if MATRIX_VALIDATION
        Volatile.Read(ref _count);
#else
        0;
#endif

    public CapturedLogEvent? Last { get; private set; }

    public override void Write(byte[] buffer, int offset, int count)
    {
#if MATRIX_VALIDATION
        lock (this)
        {
            var text = _pending + Encoding.UTF8.GetString(buffer, offset, count);
            var lines = text.Split('\n');
            for (var index = 0; index < lines.Length - 1; index++)
            {
                var line = lines[index].TrimEnd('\r');
                if (line.Length == 0)
                {
                    continue;
                }

                Interlocked.Increment(ref _count);
                Last = new CapturedLogEvent("Information", line, null, null, EmptyProperties);
            }

            _pending = lines[^1];
        }
#endif
    }

#if MATRIX_VALIDATION
    private static readonly IReadOnlyDictionary<string, object?> EmptyProperties =
        new Dictionary<string, object?>();
#endif

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => true;

    public override long Length => 0;

    public override long Position { get; set; }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count) => 0;

    public override long Seek(long offset, SeekOrigin origin) => 0;

    public override void SetLength(long value)
    {
    }
}
