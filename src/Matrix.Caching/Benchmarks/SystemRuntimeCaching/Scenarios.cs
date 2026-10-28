using System.Runtime.Caching;
// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

internal sealed class RuntimeCacheOps
{
    private readonly MemoryCache _cache = new("matrix-runtime");
    private readonly CacheItemPolicy _lifetime = new() { AbsoluteExpiration = DateTimeOffset.UtcNow.Add(CacheData.Lifetime) };

    public RuntimeCacheOps()
    {
        _cache.Set(CacheData.HitKey, CacheData.Existing, _lifetime);
        _cache.Set(CacheData.GetOrCreateKey, CacheData.Existing, _lifetime);
    }

    public CacheLookup GetHit() =>
        _cache.Get(CacheData.HitKey) is CacheEntry entry ? new CacheLookup(true, entry) : new CacheLookup(false, default);

    public CacheLookup GetMiss() =>
        _cache.Get(CacheData.MissKey) is CacheEntry entry ? new CacheLookup(true, entry) : new CacheLookup(false, default);

    public CacheLookup Set()
    {
        var key = CacheData.SetPool.Next();
        _cache.Set(key, CacheData.Created, _lifetime);
        return _cache.Get(key) is CacheEntry entry ? new CacheLookup(true, entry) : new CacheLookup(false, default);
    }

    public CacheLookup GetOrCreate()
    {
        var entry = _cache.AddOrGetExisting(CacheData.GetOrCreateKey, CacheData.Created, _lifetime);
        return entry is CacheEntry existing ? new CacheLookup(true, existing) : new CacheLookup(true, CacheData.Created);
    }

    public CacheLookup Expiration()
    {
        _cache.Set(CacheData.ExpirationKey, CacheData.Existing, new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.UtcNow.Add(CacheData.ShortLifetime),
        });
        Thread.Sleep(CacheData.Settle);
        return _cache.Get(CacheData.ExpirationKey) is CacheEntry entry ? new CacheLookup(true, entry) : new CacheLookup(false, default);
    }

    public bool Invalidate()
    {
        _cache.Set(CacheData.InvalidateKey, CacheData.InvalidationTarget, _lifetime);
        _cache.Remove(CacheData.InvalidateKey);
        return _cache.Get(CacheData.InvalidateKey) is null;
    }
}

public partial class GetHit
{
    private static readonly RuntimeCacheOps RuntimeInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemRuntimeCaching)]
    public CacheLookup SystemRuntimeCaching()
    {
        var result = RuntimeInstance.GetHit();
        CacheChecks.Hit(LibraryCatalog.SystemRuntimeCaching, result);
        return result;
    }
}

public partial class GetMiss
{
    private static readonly RuntimeCacheOps RuntimeInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemRuntimeCaching)]
    public CacheLookup SystemRuntimeCaching()
    {
        var result = RuntimeInstance.GetMiss();
        CacheChecks.Miss(LibraryCatalog.SystemRuntimeCaching, result);
        return result;
    }
}

public partial class Set
{
    private static readonly RuntimeCacheOps RuntimeInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemRuntimeCaching)]
    public CacheLookup SystemRuntimeCaching()
    {
        var result = RuntimeInstance.Set();
        CacheChecks.Written(LibraryCatalog.SystemRuntimeCaching, result);
        return result;
    }
}

public partial class GetOrCreate
{
    private static readonly RuntimeCacheOps RuntimeInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemRuntimeCaching)]
    public CacheLookup SystemRuntimeCaching()
    {
        var result = RuntimeInstance.GetOrCreate();
        CacheChecks.Hit(LibraryCatalog.SystemRuntimeCaching, result);
        return result;
    }
}

public partial class Expiration
{
    private static readonly RuntimeCacheOps RuntimeInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemRuntimeCaching)]
    public CacheLookup SystemRuntimeCaching()
    {
        var result = RuntimeInstance.Expiration();
        CacheChecks.Expired(LibraryCatalog.SystemRuntimeCaching, result);
        return result;
    }
}

public partial class Invalidate
{
    private static readonly RuntimeCacheOps RuntimeInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SystemRuntimeCaching)]
    public bool SystemRuntimeCaching()
    {
        var result = RuntimeInstance.Invalidate();
        CacheChecks.Removed(LibraryCatalog.SystemRuntimeCaching, result);
        return result;
    }
}