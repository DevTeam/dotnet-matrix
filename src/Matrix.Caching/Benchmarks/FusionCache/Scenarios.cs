using ZiggyCreatures.Caching.Fusion;
// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

internal sealed class FusionOps
{
    private readonly FusionCache _cache = new(new FusionCacheOptions());
    private readonly FusionCacheEntryOptions _lifetime = new FusionCacheEntryOptions().SetDuration(CacheData.Lifetime);

    public FusionOps() => _cache.Set(CacheData.HitKey, CacheData.Existing, _lifetime);

    public CacheLookup GetHit()
    {
        var value = _cache.TryGet<CacheEntry>(CacheData.HitKey);
        return value.HasValue ? new CacheLookup(true, value.Value) : new CacheLookup(false, default);
    }

    public CacheLookup GetMiss()
    {
        var value = _cache.TryGet<CacheEntry>(CacheData.MissKey);
        return value.HasValue ? new CacheLookup(true, value.Value) : new CacheLookup(false, default);
    }

    public CacheLookup Set()
    {
        var key = CacheData.SetPool.Next();
        _cache.Set(key, CacheData.Created, _lifetime);
        var value = _cache.TryGet<CacheEntry>(key);
        return value.HasValue ? new CacheLookup(true, value.Value) : new CacheLookup(false, default);
    }

    public CacheLookup GetOrCreate()
    {
        _cache.Set(CacheData.GetOrCreateKey, CacheData.Existing, _lifetime);
        var entry = _cache.GetOrSet<CacheEntry>(CacheData.GetOrCreateKey, _ => CacheData.Created, _lifetime);
        return new CacheLookup(true, entry);
    }

    public CacheLookup Expiration()
    {
        _cache.Set(CacheData.ExpirationKey, CacheData.Existing, new FusionCacheEntryOptions().SetDuration(CacheData.ShortLifetime));
        Thread.Sleep(CacheData.Settle);
        var value = _cache.TryGet<CacheEntry>(CacheData.ExpirationKey);
        return value.HasValue ? new CacheLookup(true, value.Value) : new CacheLookup(false, default);
    }

    public bool Invalidate()
    {
        _cache.Set(CacheData.InvalidateKey, CacheData.InvalidationTarget, _lifetime);
        _cache.Remove(CacheData.InvalidateKey);
        return !_cache.TryGet<CacheEntry>(CacheData.InvalidateKey).HasValue;
    }

    public (CacheLookup Lookup, int Runs) Concurrent()
    {
        var runs = 0;
        using var barrier = new Barrier(CacheData.StampedeCallers);
        var tasks = new Task<CacheEntry>[CacheData.StampedeCallers];
        for (var index = 0; index < tasks.Length; index++)
        {
            tasks[index] = Task.Run(() =>
            {
                barrier.SignalAndWait();
                return _cache.GetOrSet<CacheEntry>(CacheData.StampedeKey, _ =>
                {
                    Interlocked.Increment(ref runs);
                    Thread.Sleep(CacheData.FactoryDelay);
                    return CacheData.Stampede;
                }, _lifetime);
            });
        }

        Task.WaitAll(tasks);
        _cache.Remove(CacheData.StampedeKey);
        return (new CacheLookup(true, tasks[0].Result), runs);
    }

    public CacheLookup FailSafe()
    {
        var options = new FusionCacheEntryOptions()
            .SetDuration(CacheData.ShortLifetime)
            .SetFailSafe(true, CacheData.Lifetime);
        _cache.Set(CacheData.FailSafeKey, CacheData.FailSafe, options);
        Thread.Sleep(CacheData.Settle);
        var entry = _cache.GetOrSet<CacheEntry>(CacheData.FailSafeKey, _ => throw new InvalidOperationException("matrix"), options);
        return new CacheLookup(true, entry);
    }
}

public partial class GetHit
{
    private static readonly FusionOps FusionCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FusionCache)]
    public CacheLookup FusionCache()
    {
        var result = FusionCacheInstance.GetHit();
        CacheChecks.Hit(LibraryCatalog.FusionCache, result);
        return result;
    }
}

public partial class GetMiss
{
    private static readonly FusionOps FusionCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FusionCache)]
    public CacheLookup FusionCache()
    {
        var result = FusionCacheInstance.GetMiss();
        CacheChecks.Miss(LibraryCatalog.FusionCache, result);
        return result;
    }
}

public partial class Set
{
    private static readonly FusionOps FusionCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FusionCache)]
    public CacheLookup FusionCache()
    {
        var result = FusionCacheInstance.Set();
        CacheChecks.Written(LibraryCatalog.FusionCache, result);
        return result;
    }
}

public partial class GetOrCreate
{
    private static readonly FusionOps FusionCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FusionCache)]
    public CacheLookup FusionCache()
    {
        var result = FusionCacheInstance.GetOrCreate();
        CacheChecks.Hit(LibraryCatalog.FusionCache, result);
        return result;
    }
}

public partial class Expiration
{
    private static readonly FusionOps FusionCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FusionCache)]
    public CacheLookup FusionCache()
    {
        var result = FusionCacheInstance.Expiration();
        CacheChecks.Expired(LibraryCatalog.FusionCache, result);
        return result;
    }
}

public partial class Invalidate
{
    private static readonly FusionOps FusionCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FusionCache)]
    public bool FusionCache()
    {
        var result = FusionCacheInstance.Invalidate();
        CacheChecks.Removed(LibraryCatalog.FusionCache, result);
        return result;
    }
}

public partial class ConcurrentGetOrCreate
{
    private static readonly FusionOps FusionCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FusionCache)]
    public CacheLookup FusionCache()
    {
        var (lookup, runs) = FusionCacheInstance.Concurrent();
        CacheChecks.Concurrent(LibraryCatalog.FusionCache, lookup, runs);
        return lookup;
    }
}


public partial class FailSafe
{
    private static readonly FusionOps FusionCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FusionCache)]
    public CacheLookup FusionCache()
    {
        var result = FusionCacheInstance.FailSafe();
        CacheChecks.FailSafe(LibraryCatalog.FusionCache, result);
        return result;
    }
}