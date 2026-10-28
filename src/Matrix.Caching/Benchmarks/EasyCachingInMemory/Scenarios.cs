using EasyCaching.InMemory;
// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

internal sealed class EasyCachingOps
{
    private readonly InMemoryCaching _cache = new("matrix-easy", new InMemoryCachingOptions());

    public EasyCachingOps() => _cache.Set(CacheData.HitKey, CacheData.Existing, CacheData.Lifetime);

    private CacheLookup Read(string key)
    {
        var value = _cache.Get<CacheEntry>(key);
        return value.HasValue ? new CacheLookup(true, value.Value) : new CacheLookup(false, default);
    }

    public CacheLookup GetHit() => Read(CacheData.HitKey);

    public CacheLookup GetMiss() => Read(CacheData.MissKey);

    public CacheLookup Set()
    {
        var key = CacheData.SetPool.Next();
        _cache.Set(key, CacheData.Created, CacheData.Lifetime);
        return Read(key);
    }

    public CacheLookup Expiration()
    {
        _cache.Set(CacheData.ExpirationKey, CacheData.Existing, CacheData.ShortLifetime);
        Thread.Sleep(CacheData.Settle);
        return Read(CacheData.ExpirationKey);
    }

    public bool Invalidate()
    {
        _cache.Set(CacheData.InvalidateKey, CacheData.InvalidationTarget, CacheData.Lifetime);
        _cache.Remove(CacheData.InvalidateKey);
        return !_cache.Get<CacheEntry>(CacheData.InvalidateKey).HasValue;
    }
}

public partial class GetHit
{
    private static readonly EasyCachingOps EasyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.EasyCachingInMemory)]
    public CacheLookup EasyCachingInMemory()
    {
        var result = EasyInstance.GetHit();
        CacheChecks.Hit(LibraryCatalog.EasyCachingInMemory, result);
        return result;
    }
}

public partial class GetMiss
{
    private static readonly EasyCachingOps EasyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.EasyCachingInMemory)]
    public CacheLookup EasyCachingInMemory()
    {
        var result = EasyInstance.GetMiss();
        CacheChecks.Miss(LibraryCatalog.EasyCachingInMemory, result);
        return result;
    }
}

public partial class Set
{
    private static readonly EasyCachingOps EasyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.EasyCachingInMemory)]
    public CacheLookup EasyCachingInMemory()
    {
        var result = EasyInstance.Set();
        CacheChecks.Written(LibraryCatalog.EasyCachingInMemory, result);
        return result;
    }
}


public partial class Expiration
{
    private static readonly EasyCachingOps EasyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.EasyCachingInMemory)]
    public CacheLookup EasyCachingInMemory()
    {
        var result = EasyInstance.Expiration();
        CacheChecks.Expired(LibraryCatalog.EasyCachingInMemory, result);
        return result;
    }
}

public partial class Invalidate
{
    private static readonly EasyCachingOps EasyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.EasyCachingInMemory)]
    public bool EasyCachingInMemory()
    {
        var result = EasyInstance.Invalidate();
        CacheChecks.Removed(LibraryCatalog.EasyCachingInMemory, result);
        return result;
    }
}
