using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

/// <summary>
/// HybridCache has no try-read API: a read is the read-through call
/// <see cref="HybridCache.GetOrCreateAsync{T}(string, Func{CancellationToken, ValueTask{T}}, HybridCacheEntryOptions?, IEnumerable{string}?, CancellationToken)"/>.
/// A read-only lookup passes <see cref="HybridCacheEntryFlags.DisableLocalCacheWrite"/> and
/// <see cref="HybridCacheEntryFlags.DisableDistributedCacheWrite"/> — the flags a cache-only
/// lookup needs — with a static factory that returns the <see cref="Absent"/> marker. On a miss
/// the factory runs and HybridCache does not store its result, so the marker reports absence;
/// nothing is written and no sentinel is ever cached. Comparing against 
/// <see cref="HybridCacheEntryFlags.DisableUnderlyingData"/> alone was rejected: that flag
/// skips the factory and returns default(T), which the category already treats as an
/// unobservable miss (see CacheManager.Core GetMiss). Options instances and the factories are
/// created once, outside the measured method.
/// </summary>
internal sealed class HybridOps
{
    private static readonly CacheEntry Absent = new(-1, -1, -1);

    private readonly HybridCache _cache;
    private readonly HybridCacheEntryOptions _lifetime = new() { Expiration = CacheData.Lifetime };
    private readonly HybridCacheEntryOptions _shortLifetime = new() { Expiration = CacheData.ShortLifetime };
    private readonly HybridCacheEntryOptions _lookup = new()
    {
        Flags = HybridCacheEntryFlags.DisableLocalCacheWrite | HybridCacheEntryFlags.DisableDistributedCacheWrite,
    };

    public HybridOps()
    {
        var services = new ServiceCollection();
        services.AddHybridCache();
        _cache = services.BuildServiceProvider().GetRequiredService<HybridCache>();
        Store(CacheData.HitKey, CacheData.Existing, _lifetime);
    }

    public CacheLookup GetHit() => Lookup(CacheData.HitKey);

    public CacheLookup GetMiss() => Lookup(CacheData.MissKey);

    public CacheLookup Set()
    {
        var key = CacheData.SetPool.Next();
        Store(key, CacheData.Created, _lifetime);
        return Lookup(key);
    }

    public CacheLookup GetOrCreate()
    {
        Store(CacheData.GetOrCreateKey, CacheData.Existing, _lifetime);
        var entry = _cache.GetOrCreateAsync<CacheEntry>(CacheData.GetOrCreateKey, static _ => ValueTask.FromResult(CacheData.Created), _lifetime, null, default)
            .AsTask().GetAwaiter().GetResult();
        return new CacheLookup(true, entry);
    }

    public CacheLookup Expiration()
    {
        Store(CacheData.ExpirationKey, CacheData.Existing, _shortLifetime);
        Thread.Sleep(CacheData.Settle);
        return Lookup(CacheData.ExpirationKey);
    }

    public bool Invalidate()
    {
        Store(CacheData.InvalidateKey, CacheData.InvalidationTarget, _lifetime);
        _cache.RemoveAsync(CacheData.InvalidateKey).AsTask().GetAwaiter().GetResult();
        return !Lookup(CacheData.InvalidateKey).Found;
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
                return _cache.GetOrCreateAsync<CacheEntry>(CacheData.StampedeKey, _ =>
                    {
                        Interlocked.Increment(ref runs);
                        Thread.Sleep(CacheData.FactoryDelay);
                        return ValueTask.FromResult(CacheData.Stampede);
                    }, _lifetime, null, default)
                    .AsTask().GetAwaiter().GetResult();
            });
        }

        Task.WaitAll(tasks);
        _cache.RemoveAsync(CacheData.StampedeKey).AsTask().GetAwaiter().GetResult();
        return (new CacheLookup(true, tasks[0].Result), runs);
    }

    private void Store(string key, CacheEntry entry, HybridCacheEntryOptions options) =>
        _cache.SetAsync(key, entry, options, null, default).AsTask().GetAwaiter().GetResult();

    private CacheLookup Lookup(string key)
    {
        var value = _cache.GetOrCreateAsync<CacheEntry>(key, static _ => ValueTask.FromResult(Absent), _lookup, null, default)
            .AsTask().GetAwaiter().GetResult();
        return value != Absent ? new CacheLookup(true, value) : new CacheLookup(false, default);
    }
}

public partial class GetHit
{
    private static readonly HybridOps HybridInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingHybrid)]
    public CacheLookup MicrosoftExtensionsCachingHybrid()
    {
        var result = HybridInstance.GetHit();
        CacheChecks.Hit(LibraryCatalog.MicrosoftExtensionsCachingHybrid, result);
        return result;
    }
}

public partial class GetMiss
{
    private static readonly HybridOps HybridInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingHybrid)]
    public CacheLookup MicrosoftExtensionsCachingHybrid()
    {
        var result = HybridInstance.GetMiss();
        CacheChecks.Miss(LibraryCatalog.MicrosoftExtensionsCachingHybrid, result);
        return result;
    }
}

public partial class Set
{
    private static readonly HybridOps HybridInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingHybrid)]
    public CacheLookup MicrosoftExtensionsCachingHybrid()
    {
        var result = HybridInstance.Set();
        CacheChecks.Written(LibraryCatalog.MicrosoftExtensionsCachingHybrid, result);
        return result;
    }
}

public partial class GetOrCreate
{
    private static readonly HybridOps HybridInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingHybrid)]
    public CacheLookup MicrosoftExtensionsCachingHybrid()
    {
        var result = HybridInstance.GetOrCreate();
        CacheChecks.Hit(LibraryCatalog.MicrosoftExtensionsCachingHybrid, result);
        return result;
    }
}

public partial class Expiration
{
    private static readonly HybridOps HybridInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingHybrid)]
    public CacheLookup MicrosoftExtensionsCachingHybrid()
    {
        var result = HybridInstance.Expiration();
        CacheChecks.Expired(LibraryCatalog.MicrosoftExtensionsCachingHybrid, result);
        return result;
    }
}

public partial class Invalidate
{
    private static readonly HybridOps HybridInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingHybrid)]
    public bool MicrosoftExtensionsCachingHybrid()
    {
        var result = HybridInstance.Invalidate();
        CacheChecks.Removed(LibraryCatalog.MicrosoftExtensionsCachingHybrid, result);
        return result;
    }
}

public partial class ConcurrentGetOrCreate
{
    private static readonly HybridOps HybridInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingHybrid)]
    public CacheLookup MicrosoftExtensionsCachingHybrid()
    {
        var (lookup, runs) = HybridInstance.Concurrent();
        CacheChecks.Concurrent(LibraryCatalog.MicrosoftExtensionsCachingHybrid, lookup, runs);
        return lookup;
    }
}
