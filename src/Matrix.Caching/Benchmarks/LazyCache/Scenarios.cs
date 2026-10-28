using Microsoft.Extensions.Caching.Memory;
using LazyCache;
// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

internal sealed class LazyCacheOps
{
    private readonly CachingService _cache = new();
    private readonly MemoryCacheEntryOptions _lifetime = new() { AbsoluteExpirationRelativeToNow = CacheData.Lifetime };

    public LazyCacheOps() => _cache.Add(CacheData.HitKey, CacheData.Existing, _lifetime);

    public CacheLookup GetHit() =>
        _cache.TryGetValue(CacheData.HitKey, out CacheEntry entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);

    public CacheLookup GetMiss() =>
        _cache.TryGetValue(CacheData.MissKey, out CacheEntry entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);

    public CacheLookup Set()
    {
        var key = CacheData.SetPool.Next();
        _cache.Add(key, CacheData.Created, _lifetime);
        return _cache.TryGetValue(key, out CacheEntry entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);
    }

    public CacheLookup GetOrCreate()
    {
        _cache.Add(CacheData.GetOrCreateKey, CacheData.Existing, _lifetime);
        var entry = _cache.GetOrAdd(CacheData.GetOrCreateKey, _ => CacheData.Created);
        return new CacheLookup(true, entry);
    }

    public CacheLookup Expiration()
    {
        _cache.Add(CacheData.ExpirationKey, CacheData.Existing, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheData.ShortLifetime,
        });
        Thread.Sleep(CacheData.Settle);
        return _cache.TryGetValue(CacheData.ExpirationKey, out CacheEntry entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);
    }

    public bool Invalidate()
    {
        _cache.Add(CacheData.InvalidateKey, CacheData.InvalidationTarget, _lifetime);
        _cache.Remove(CacheData.InvalidateKey);
        return !_cache.TryGetValue<CacheEntry>(CacheData.InvalidateKey, out _);
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
                return _cache.GetOrAdd(CacheData.StampedeKey, _ =>
                {
                    Interlocked.Increment(ref runs);
                    Thread.Sleep(CacheData.FactoryDelay);
                    return CacheData.Stampede;
                });
            });
        }

        Task.WaitAll(tasks);
        _cache.Remove(CacheData.StampedeKey);
        return (new CacheLookup(true, tasks[0].Result), runs);
    }
}

public partial class GetHit
{
    private static readonly LazyCacheOps LazyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LazyCache)]
    public CacheLookup LazyCache()
    {
        var result = LazyInstance.GetHit();
        CacheChecks.Hit(LibraryCatalog.LazyCache, result);
        return result;
    }
}

public partial class GetMiss
{
    private static readonly LazyCacheOps LazyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LazyCache)]
    public CacheLookup LazyCache()
    {
        var result = LazyInstance.GetMiss();
        CacheChecks.Miss(LibraryCatalog.LazyCache, result);
        return result;
    }
}

public partial class Set
{
    private static readonly LazyCacheOps LazyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LazyCache)]
    public CacheLookup LazyCache()
    {
        var result = LazyInstance.Set();
        CacheChecks.Written(LibraryCatalog.LazyCache, result);
        return result;
    }
}

public partial class GetOrCreate
{
    private static readonly LazyCacheOps LazyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LazyCache)]
    public CacheLookup LazyCache()
    {
        var result = LazyInstance.GetOrCreate();
        CacheChecks.Hit(LibraryCatalog.LazyCache, result);
        return result;
    }
}

public partial class Expiration
{
    private static readonly LazyCacheOps LazyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LazyCache)]
    public CacheLookup LazyCache()
    {
        var result = LazyInstance.Expiration();
        CacheChecks.Expired(LibraryCatalog.LazyCache, result);
        return result;
    }
}

public partial class Invalidate
{
    private static readonly LazyCacheOps LazyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LazyCache)]
    public bool LazyCache()
    {
        var result = LazyInstance.Invalidate();
        CacheChecks.Removed(LibraryCatalog.LazyCache, result);
        return result;
    }
}

public partial class ConcurrentGetOrCreate
{
    private static readonly LazyCacheOps LazyInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.LazyCache)]
    public CacheLookup LazyCache()
    {
        var (lookup, runs) = LazyInstance.Concurrent();
        CacheChecks.Concurrent(LibraryCatalog.LazyCache, lookup, runs);
        return lookup;
    }
}