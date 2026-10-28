namespace Matrix.CompressionCodecs;

/// <summary>
/// The levels each codec is measured at, stated once. The contract binds every
/// participant to these numbers so that a time comparison is also a comparison
/// of the same requested effort: zstd level 1 is the library's own fastest and
/// level 19 its documented maximum for general data, LZ4 and Brotli use the
/// levels named by their public APIs.
/// </summary>
internal static class CompressionLevels
{
    /// <summary>zstd's fastest documented level.</summary>
    public const int ZstdFastest = 1;

    /// <summary>
    /// zstd's highest level intended for general input; higher numbers only add
    /// window size that this corpus cannot use.
    /// </summary>
    public const int ZstdMaximum = 19;

    /// <summary>Brotli's fastest quality.</summary>
    public const int BrotliFastest = 1;

    /// <summary>Brotli's maximum quality.</summary>
    public const int BrotliMaximum = 11;
}
