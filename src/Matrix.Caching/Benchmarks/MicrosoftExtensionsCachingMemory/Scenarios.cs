using Microsoft.Extensions.Caching.Memory;
// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

internal sealed class MemoryOps
{
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private readonly MemoryCacheEntryOptions _lifetime = new() { AbsoluteExpirationRelativeToNow = CacheData.Lifetime };

    public MemoryOps() => _cache.Set(CacheData.HitKey, CacheData.Existing, _lifetime);

    public CacheLookup GetHit() =>
        _cache.TryGetValue(CacheData.HitKey, out CacheEntry entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);

    public CacheLookup GetMiss() =>
        _cache.TryGetValue(CacheData.MissKey, out CacheEntry entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);

    public CacheLookup Set()
    {
        var key = CacheData.SetPool.Next();
        _cache.Set(key, CacheData.Created, _lifetime);
        return _cache.TryGetValue(key, out CacheEntry entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);
    }

    public CacheLookup GetOrCreate()
    {
        _cache.Set(CacheData.GetOrCreateKey, CacheData.Existing, _lifetime);
        return new CacheLookup(true, _cache.GetOrCreate(CacheData.GetOrCreateKey, _ => CacheData.Created));
    }

    public CacheLookup Expiration()
    {
        _cache.Set(CacheData.ExpirationKey, CacheData.Existing, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheData.ShortLifetime });
        Thread.Sleep(CacheData.Settle);
        return _cache.TryGetValue(CacheData.ExpirationKey, out CacheEntry entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);
    }

    public bool Invalidate()
    {
        _cache.Set(CacheData.InvalidateKey, CacheData.InvalidationTarget, _lifetime);
        _cache.Remove(CacheData.InvalidateKey);
        return !_cache.TryGetValue(CacheData.InvalidateKey, out _);
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
                return _cache.GetOrCreate(CacheData.StampedeKey, _ =>
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
    private static readonly MemoryOps MemoryCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingMemory)]
    public CacheLookup MicrosoftExtensionsCachingMemory()
    {
        var result = MemoryCacheInstance.GetHit();
        CacheChecks.Hit(LibraryCatalog.MicrosoftExtensionsCachingMemory, result);
        return result;
    }
}

public partial class GetMiss
{
    private static readonly MemoryOps MemoryCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingMemory)]
    public CacheLookup MicrosoftExtensionsCachingMemory()
    {
        var result = MemoryCacheInstance.GetMiss();
        CacheChecks.Miss(LibraryCatalog.MicrosoftExtensionsCachingMemory, result);
        return result;
    }
}

public partial class Set
{
    private static readonly MemoryOps MemoryCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingMemory)]
    public CacheLookup MicrosoftExtensionsCachingMemory()
    {
        var result = MemoryCacheInstance.Set();
        CacheChecks.Written(LibraryCatalog.MicrosoftExtensionsCachingMemory, result);
        return result;
    }
}

public partial class GetOrCreate
{
    private static readonly MemoryOps MemoryCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingMemory)]
    public CacheLookup MicrosoftExtensionsCachingMemory()
    {
        var result = MemoryCacheInstance.GetOrCreate();
        CacheChecks.Hit(LibraryCatalog.MicrosoftExtensionsCachingMemory, result);
        return result;
    }
}

public partial class Expiration
{
    private static readonly MemoryOps MemoryCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingMemory)]
    public CacheLookup MicrosoftExtensionsCachingMemory()
    {
        var result = MemoryCacheInstance.Expiration();
        CacheChecks.Expired(LibraryCatalog.MicrosoftExtensionsCachingMemory, result);
        return result;
    }
}

public partial class Invalidate
{
    private static readonly MemoryOps MemoryCacheInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MicrosoftExtensionsCachingMemory)]
    public bool MicrosoftExtensionsCachingMemory()
    {
        var result = MemoryCacheInstance.Invalidate();
        CacheChecks.Removed(LibraryCatalog.MicrosoftExtensionsCachingMemory, result);
        return result;
    }
}

