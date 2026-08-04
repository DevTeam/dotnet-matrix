using System.Buffers;
using System.Text;

namespace Matrix.Logging;

internal sealed class FormattedOutputSink : TextWriter, IBufferWriter<byte>
{
    private char[] _characters = new char[256];
    private byte[] _bytes = new byte[256];
    private int _characterCount;
    private int _byteCount;

    public override Encoding Encoding => Encoding.UTF8;

    public long Count { get; private set; }

    public string? Last { get; private set; }

    public void BeginRecord()
    {
        _characterCount = 0;
        _byteCount = 0;
    }

    public void EndTextRecord()
    {
        Count++;
#if MATRIX_VALIDATION
        Last = new string(_characters, 0, _characterCount);
#endif
    }

    public void EndUtf8Record()
    {
        Count++;
#if MATRIX_VALIDATION
        Last = Encoding.UTF8.GetString(_bytes, 0, _byteCount);
#endif
    }

    public override void Write(char value)
    {
        EnsureCharacterCapacity(1);
        _characters[_characterCount++] = value;
    }

    public override void Write(string? value)
    {
        if (value is not null)
        {
            Write(value.AsSpan());
        }
    }

    public override void Write(ReadOnlySpan<char> buffer)
    {
        EnsureCharacterCapacity(buffer.Length);
        buffer.CopyTo(_characters.AsSpan(_characterCount));
        _characterCount += buffer.Length;
    }

    public void Advance(int count)
    {
        if (count < 0 || _byteCount > _bytes.Length - count)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        _byteCount += count;
    }

    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        EnsureByteCapacity(sizeHint);
        return _bytes.AsMemory(_byteCount);
    }

    public Span<byte> GetSpan(int sizeHint = 0)
    {
        EnsureByteCapacity(sizeHint);
        return _bytes.AsSpan(_byteCount);
    }

    private void EnsureCharacterCapacity(int additionalCount)
    {
        var required = checked(_characterCount + additionalCount);
        if (required > _characters.Length)
        {
            Array.Resize(ref _characters, Math.Max(required, _characters.Length * 2));
        }
    }

    private void EnsureByteCapacity(int sizeHint)
    {
        var required = checked(_byteCount + Math.Max(sizeHint, 1));
        if (required > _bytes.Length)
        {
            Array.Resize(ref _bytes, Math.Max(required, _bytes.Length * 2));
        }
    }
}
