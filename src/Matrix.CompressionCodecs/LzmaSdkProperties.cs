namespace Matrix.CompressionCodecs;

/// <summary>
/// The LZMA-SDK coder properties behind the two compression levels this
/// category compares, stated once and built outside every measured call. The
/// package exposes the 7-Zip LZMA encoder through
/// <c>SetCoderProperties</c>, which takes property identifiers and boxed
/// values: both arrays are static so the boxing happens at type
/// initialization instead of inside a benchmark.
/// </summary>
/// <remarks>
/// The identifiers are the only ones this SDK version accepts for the LZMA
/// encoder: the dictionary size, the algorithm (0 = fast mode, 1 = normal
/// mode) and the number of fast bytes. Level 0 of the seven-zip command line
/// is fast mode with 32 fast bytes, level 9 is normal mode with 273 fast
/// bytes, and those are the two ends the contract asks for.
/// </remarks>
internal static class LzmaSdkProperties
{
    /// <summary>
    /// 19-bit dictionary, the size of the largest corpus this category
    /// compresses. A larger dictionary only reserves memory that a 256 KiB
    /// payload cannot use.
    /// </summary>
    public const int DictionarySize = 1 << 18;

    /// <summary>LZMA fast mode, the encoder's <c>Algorithm</c> value 0.</summary>
    public const int FastAlgorithm = 0;

    /// <summary>LZMA normal mode, the encoder's <c>Algorithm</c> value 1.</summary>
    public const int NormalAlgorithm = 1;

    /// <summary>The fast bytes count of the fastest level.</summary>
    public const int FastBytesFastest = 32;

    /// <summary>The largest fast bytes count the encoder accepts.</summary>
    public const int FastBytesMaximum = 273;

    public static readonly SevenZip.CoderPropID[] EncoderPropertyIds =
    [
        SevenZip.CoderPropID.DictionarySize,
        SevenZip.CoderPropID.Algorithm,
        SevenZip.CoderPropID.NumFastBytes
    ];

    /// <summary>The properties of the level the contract calls fastest.</summary>
    public static readonly object[] Fastest =
    [
        DictionarySize,
        FastAlgorithm,
        FastBytesFastest
    ];

    /// <summary>The properties of the level the contract calls maximum effort.</summary>
    public static readonly object[] Maximum =
    [
        DictionarySize,
        NormalAlgorithm,
        FastBytesMaximum
    ];
}
