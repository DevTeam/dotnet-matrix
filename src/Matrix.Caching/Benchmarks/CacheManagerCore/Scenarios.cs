using CacheManager.Core;
// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

internal sealed class CacheManagerOps
{
    private readonly ICacheManager<CacheEntry> _cache =
        CacheFactory.Build<CacheEntry>("matrix-cache-manager", settings => settings.WithDictionaryHandle());

    private readonly CacheItem<CacheEntry> _hitItem;
    private readonly CacheItem<CacheEntry> _invalidateItem;

    public CacheManagerOps()
    {
        _hitItem = new CacheItem<CacheEntry>(CacheData.HitKey, CacheData.Existing, ExpirationMode.Absolute, CacheData.Lifetime);
        _invalidateItem = new CacheItem<CacheEntry>(CacheData.InvalidateKey, CacheData.InvalidationTarget, ExpirationMode.Absolute, CacheData.Lifetime);
        _cache.Add(_hitItem);
    }

    public CacheLookup GetHit() => new(true, _cache.Get(CacheData.HitKey));

    public CacheLookup Set()
    {
        var item = new CacheItem<CacheEntry>(CacheData.SetPool.Next(), CacheData.Created, ExpirationMode.Absolute, CacheData.Lifetime);
        _cache.Add(item);
        return new CacheLookup(true, _cache.Get(item.Key));
    }

    public CacheLookup Expiration()
    {
        var item = new CacheItem<CacheEntry>(CacheData.ExpirationKey, CacheData.Existing, ExpirationMode.Absolute, CacheData.ShortLifetime);
        _cache.Add(item);
        Thread.Sleep(CacheData.Settle);
        var value = _cache.Get(CacheData.ExpirationKey);
        return value.Equals(default(CacheEntry)) ? new CacheLookup(false, default) : new CacheLookup(true, value);
    }

    public bool Invalidate()
    {
        _cache.Add(_invalidateItem);
        _cache.Remove(CacheData.InvalidateKey);
        return _cache.Get(CacheData.InvalidateKey).Equals(default(CacheEntry));
    }
}

public partial class GetHit
{
    private static readonly CacheManagerOps ManagerInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CacheManagerCore)]
    public CacheLookup CacheManagerCore()
    {
        var result = ManagerInstance.GetHit();
        CacheChecks.Hit(LibraryCatalog.CacheManagerCore, result);
        return result;
    }
}

public partial class Set
{
    private static readonly CacheManagerOps ManagerInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CacheManagerCore)]
    public CacheLookup CacheManagerCore()
    {
        var result = ManagerInstance.Set();
        CacheChecks.Written(LibraryCatalog.CacheManagerCore, result);
        return result;
    }
}

public partial class Expiration
{
    private static readonly CacheManagerOps ManagerInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CacheManagerCore)]
    public CacheLookup CacheManagerCore()
    {
        var result = ManagerInstance.Expiration();
        CacheChecks.Expired(LibraryCatalog.CacheManagerCore, result);
        return result;
    }
}

public partial class Invalidate
{
    private static readonly CacheManagerOps ManagerInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CacheManagerCore)]
    public bool CacheManagerCore()
    {
        var result = ManagerInstance.Invalidate();
        CacheChecks.Removed(LibraryCatalog.CacheManagerCore, result);
        return result;
    }
}