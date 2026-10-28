namespace Matrix.CompressionCodecs;

/// <summary>
/// The single deterministic corpus every codec receives. It is part of the
/// category contract, not an implementation detail: a codec result is only
/// comparable with another codec result when both compressed the same bytes.
/// </summary>
internal static class CompressionData
{
    /// <summary>
    /// Highly repetitive text, the shape real logs and documents take. Small
    /// enough that per-call overhead is visible, large enough that throughput
    /// dominates it.
    /// </summary>
    public const int CompressibleLength = 256 * 1024;

    /// <summary>
    /// The same size, but incompressible. A codec that behaves well on text can
    /// still lose badly here, and that is the point of measuring it.
    /// </summary>
    public const int IncompressibleLength = 256 * 1024;

    public const int SmallLength = 1024;

    private static readonly Lazy<byte[]> CompressibleValue = new(() => CreateText(CompressibleLength));
    private static readonly Lazy<byte[]> IncompressibleValue = new(() => CreateRandom(IncompressibleLength, 73));
    private static readonly Lazy<byte[]> SmallValue = new(() => CreateText(SmallLength));

    /// <summary>The corpus compressed at the <c>Fastest</c> level, for decompression.</summary>
    private static readonly Lazy<byte[]> SmallPayloadValue = new(() => CreateText(SmallLength));

    public static byte[] Compressible => CompressibleValue.Value;

    public static byte[] Incompressible => IncompressibleValue.Value;

    public static byte[] Small => SmallValue.Value;

    /// <summary>
    /// Upper bound for a destination buffer and for validation. Snappy is the
    /// widest of the six here: its documented bound is <c>32 + n + n/6</c>, and a
    /// destination sized for a tighter codec would overflow on random input.
    /// </summary>
    public static int MaximumCompressedLength(int sourceLength) =>
        sourceLength + (sourceLength / 6) + 64;

    private static byte[] CreateText(int length)
    {
        var pattern = "The quick brown fox jumps over the lazy dog. 0123456789\n"u8.ToArray();
        var result = new byte[length];
        for (var offset = 0; offset < result.Length; offset += pattern.Length)
        {
            pattern.AsSpan(0, Math.Min(pattern.Length, result.Length - offset))
                .CopyTo(result.AsSpan(offset));
        }

        return result;
    }

    private static byte[] CreateRandom(int length, int seed)
    {
        var result = new byte[length];
        new Random(seed).NextBytes(result);
        return result;
    }
}
