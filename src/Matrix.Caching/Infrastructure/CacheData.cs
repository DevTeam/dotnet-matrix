namespace Matrix.Caching.Infrastructure;

/// <summary>
/// Every key, value and lifetime the scenarios share. Keys and entries are
/// created once so a measured operation never formats a string or allocates a
/// payload; see workflows/feature-contracts/caching.md.
/// </summary>
internal static class CacheData
{
    public const string HitKey = "matrix/hit";
    public const string MissKey = "matrix/miss";
    public const string GetOrCreateKey = "matrix/get-or-create";
    public const string ExpirationKey = "matrix/expiration";
    public const string InvalidateKey = "matrix/invalidate";
    public const string FailSafeKey = "matrix/fail-safe";
    public const string StampedeKey = "matrix/stampede";

    public const int StampedeCallers = 8;

    public static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(10);
    public static readonly TimeSpan ShortLifetime = TimeSpan.FromMilliseconds(50);
    public static readonly TimeSpan Settle = TimeSpan.FromMilliseconds(200);
    public static readonly TimeSpan FactoryDelay = TimeSpan.FromMilliseconds(2);

    public static readonly CacheEntry Existing = new(42, 1_700_000_000, 7);
    public static readonly CacheEntry Created = new(43, 1_700_000_100, 8);
    public static readonly CacheEntry InvalidationTarget = new(46, 1_700_000_400, 11);
    public static readonly CacheEntry FailSafe = new(44, 1_700_000_200, 9);
    public static readonly CacheEntry Stampede = new(45, 1_700_000_300, 10);

    public const int StampedeIndex = 5;

    private static int _nextIndex = 1_000_000;

    /// <summary>Numeric keys for caches that are keyed by int rather than by string.</summary>
    public static int NextIndex() => Interlocked.Increment(ref _nextIndex);

    /// <summary>Keys for writes, so an insertion never overwrites what a read scenario seeded.</summary>
    public static readonly KeyPool SetPool = new("matrix/set/", 65_536);
}
