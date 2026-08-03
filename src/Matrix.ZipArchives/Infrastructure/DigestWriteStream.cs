namespace Matrix.ZipArchives.Infrastructure;

internal sealed class DigestWriteStream : Stream
{
    private const uint Offset = 2_166_136_261;
    private const uint Prime = 16_777_619;

    private uint _hash = Offset;
    private long _length;

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => true;

    public override long Length => _length;

    public override long Position
    {
        get => Length;
        set => throw new NotSupportedException();
    }

    public uint Hash => _hash;

    public void Reset()
    {
        _length = 0;
        _hash = Offset;
    }

    public override void Write(byte[] buffer, int offset, int count) =>
        Write(buffer.AsSpan(offset, count));

    public override void Write(ReadOnlySpan<byte> buffer)
    {
        foreach (var value in buffer)
        {
            _hash = (_hash ^ value) * Prime;
        }

        _length += buffer.Length;
    }

    public override void WriteByte(byte value)
    {
        _hash = (_hash ^ value) * Prime;
        _length++;
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) =>
        throw new NotSupportedException();

    public override void SetLength(long value) =>
        throw new NotSupportedException();
}
