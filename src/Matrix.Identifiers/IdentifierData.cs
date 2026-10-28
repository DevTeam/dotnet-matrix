namespace Matrix.Identifiers;

internal static class IdentifierData
{
    public const int BatchSize = 1_024;
    public const int UlidLength = 26;
    public const int GuidLength = 36;
    public const int KsuidLength = 27;
    public const int RandomLength = 21;
    public const int OrderingSamples = 256;
    public const long ClockToleranceMilliseconds = 60_000;

    public const uint KsuidTimestamp = 1_000u;

    // KSUID counts seconds from 2014-05-13T00:00:00Z.
    public const long KsuidEpochSeconds = 1_400_000_000;

    public static readonly DateTimeOffset BaseTimestamp =
        new(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public static readonly long FixedTimestampMilliseconds = BaseTimestamp.ToUnixTimeMilliseconds();

    public static readonly long[] Numbers = [1, 42, 1000, 999999];

    // Time-ordered libraries keep a process-wide monotonic guard and reject a
    // timestamp earlier than the previous one, so every sample comes from one
    // strictly increasing sequence regardless of scenario execution order.
    private static long _timestampOffset;

    public static long NextTimestampMilliseconds() =>
        FixedTimestampMilliseconds + Interlocked.Increment(ref _timestampOffset);

    public static DateTimeOffset NextTimestamp() =>
        DateTimeOffset.FromUnixTimeMilliseconds(NextTimestampMilliseconds());

    public static long NowMilliseconds() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    public static IdGen.IdGenerator CreateGenerator() => new(1);
}
