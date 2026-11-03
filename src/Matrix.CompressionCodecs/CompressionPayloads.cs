namespace Matrix.CompressionCodecs;

/// <summary>
/// Builds the compressed input a decompression scenario consumes. Each builder
/// runs once in a benchmark setup, never inside a timed call, and each codec's
/// payload is produced by that same codec because no decoder reads a foreign
/// format. The source and the verified result are always the shared
/// <see cref="CompressionData"/> corpus.
/// </summary>
internal static class CompressionPayloads
{
    public static byte[] CreateBrotli(byte[] source)
    {
        using var destination = new MemoryStream();
        using (var encoder = new System.IO.Compression.BrotliStream(
                   destination,
                   System.IO.Compression.CompressionLevel.SmallestSize,
                   true))
        {
            encoder.Write(source);
        }

        return destination.ToArray();
    }

    public static byte[] CreateBrotliSharp(byte[] source) =>
        BrotliSharpLib.Brotli.CompressBuffer(
            source,
            0,
            source.Length,
            CompressionLevels.BrotliMaximum,
            22,
            null!);

    public static byte[] CreateZstdSharp(byte[] source)
    {
        using var encoder = new ZstdSharp.Compressor(CompressionLevels.ZstdMaximum);
        var target = new byte[ZstdSharp.Compressor.GetCompressBound(source.Length)];
        var size = encoder.Wrap(source, target);
        return target[..size];
    }

    public static byte[] CreateZstdNet(byte[] source)
    {
        using var encoder = new ZstdNet.Compressor(
            new ZstdNet.CompressionOptions(CompressionLevels.ZstdMaximum));
        return encoder.Wrap(source);
    }

    public static byte[] CreateLz4(byte[] source)
    {
        var target = new byte[K4os.Compression.LZ4.LZ4Codec.MaximumOutputSize(source.Length)];
        var size = K4os.Compression.LZ4.LZ4Codec.Encode(
            source,
            target,
            K4os.Compression.LZ4.LZ4Level.L12_MAX);
        return target[..size];
    }

    public static byte[] CreateSnappy(byte[] source) =>
        Snappier.Snappy.CompressToArray(source);

    /// <summary>
    /// The LZMA format carries no framing: a decoder must be told the coder
    /// properties and the exact output size before it reads a byte, so both
    /// travel with the payload. The five property bytes come first, the raw
    /// LZMA stream follows, and <see cref="CreateLzmaSdk"/> is the only
    /// producer, which keeps that layout in one place.
    /// </summary>
    public static byte[] CreateLzmaSdk(byte[] source)
    {
        var encoder = new SevenZip.Compression.LZMA.Encoder();
        encoder.SetCoderProperties(LzmaSdkProperties.EncoderPropertyIds, LzmaSdkProperties.Maximum);

        using var destination = new MemoryStream();
        encoder.WriteCoderProperties(destination);
        using (var input = new MemoryStream(source, false))
        {
            encoder.Code(input, destination, source.Length, -1, null!);
        }

        return destination.ToArray();
    }

    /// <summary>The LZMA coder properties prefix of a payload this class built.</summary>
    public const int LzmaSdkPropertiesLength = 5;

    /// <summary>
    /// A destination bound for decoders that need the expected output size up
    /// front and cannot read it from the payload, such as the LZ4 block format.
    /// </summary>
    public static int MaximumDecompressedLength(int sourceLength) => sourceLength + 1024;
}
