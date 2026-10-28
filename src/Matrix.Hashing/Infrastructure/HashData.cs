namespace Matrix.Hashing.Infrastructure;

/// <summary>
/// The two deterministic corpora and the expected result of every scenario, all
/// computed before measurement and never inside a measured method. See
/// workflows/feature-contracts/hashing.md.
/// </summary>
internal static class HashData
{
    private const ulong SmallSeed = 0x5EED0017;
    private const ulong LargeSeed = 0x5EED0029;

    private static readonly Lazy<byte[]> SmallValue = new(() => Create(4 * 1024, SmallSeed));
    private static readonly Lazy<byte[]> LargeValue = new(() => Create(1024 * 1024, LargeSeed));

    public const int StreamingChunkLength = 64;

    public const ulong XxHash64Expected = 0x1A0B389290FC289C;
    public const uint XxHash32Expected = 0xF6B12790;
    public const ulong XxHash3Expected = 0x6F8AD6DCA1955037;
    public const ulong XxHash64LargeExpected = 0x9D79E17926422723;
    public const uint Crc32Expected = 0xD66510DA;
    public const ulong Blake3Expected = 0xC530EE8E61AB2D6A;
    public const ulong Blake2bExpected = 0x66F2A98E1EA7D5C9;
    public const ulong FarmHash64Expected = 0xAA31360635666EC3;

    public static byte[] Small => SmallValue.Value;

    public static byte[] Large => LargeValue.Value;

    /// <summary>
    /// A 64-bit linear congruential generator, so every machine and every run
    /// hashes identical bytes without storing a megabyte in the repository.
    /// </summary>
    private static byte[] Create(int length, ulong seed)
    {
        var result = new byte[length];
        var state = seed;
        for (var index = 0; index < length; index++)
        {
            state = unchecked((state * 6364136223846793005UL) + 1442695040888963407UL);
            result[index] = (byte)(state >> 56);
        }

        return result;
    }
}
