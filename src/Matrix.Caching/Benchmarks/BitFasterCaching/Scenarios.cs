using BitFaster.Caching.Lru;
// ReSharper disable CheckNamespace
namespace Matrix.Caching.Benchmarks;

internal sealed class BitFasterOps
{
    private readonly ConcurrentLru<int, CacheEntry> _cache = new(1024);

    private const int HitKey = 1;
    private const int MissKey = 2;
    private const int GetOrCreateKey = 3;
    private const int InvalidateKey = 4;

    public BitFasterOps() => _cache.AddOrUpdate(HitKey, CacheData.Existing);

    public CacheLookup GetHit() =>
        _cache.TryGet(HitKey, out var entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);

    public CacheLookup GetMiss() =>
        _cache.TryGet(MissKey, out var entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);

    public CacheLookup Set()
    {
        var key = CacheData.NextIndex();
        _cache.AddOrUpdate(key, CacheData.Created);
        return _cache.TryGet(key, out var entry) ? new CacheLookup(true, entry) : new CacheLookup(false, default);
    }

    public CacheLookup GetOrCreate()
    {
        _cache.AddOrUpdate(GetOrCreateKey, CacheData.Existing);
        return new CacheLookup(true, _cache.GetOrAdd(GetOrCreateKey, _ => CacheData.Created));
    }

    public bool Invalidate()
    {
        _cache.AddOrUpdate(InvalidateKey, CacheData.InvalidationTarget);
        _cache.TryRemove(InvalidateKey);
        return !_cache.TryGet(InvalidateKey, out _);
    }

    public (CacheLookup Lookup, int Runs) Concurrent()
    {
        var runs = 0;
        var key = CacheData.StampedeIndex;
        using var barrier = new Barrier(CacheData.StampedeCallers);
        var tasks = new Task<CacheEntry>[CacheData.StampedeCallers];
        for (var index = 0; index < tasks.Length; index++)
        {
            tasks[index] = Task.Run(() =>
            {
                barrier.SignalAndWait();
                return _cache.GetOrAdd(key, _ =>
                {
                    Interlocked.Increment(ref runs);
                    Thread.Sleep(CacheData.FactoryDelay);
                    return CacheData.Stampede;
                });
            });
        }

        Task.WaitAll(tasks);
        _cache.TryRemove(key);
        return (new CacheLookup(true, tasks[0].Result), runs);
    }
}

public partial class GetHit
{
    private static readonly BitFasterOps BitFasterInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.BitFasterCaching)]
    public CacheLookup BitFasterCaching()
    {
        var result = BitFasterInstance.GetHit();
        CacheChecks.Hit(LibraryCatalog.BitFasterCaching, result);
        return result;
    }
}

public partial class GetMiss
{
    private static readonly BitFasterOps BitFasterInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.BitFasterCaching)]
    public CacheLookup BitFasterCaching()
    {
        var result = BitFasterInstance.GetMiss();
        CacheChecks.Miss(LibraryCatalog.BitFasterCaching, result);
        return result;
    }
}

public partial class Set
{
    private static readonly BitFasterOps BitFasterInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.BitFasterCaching)]
    public CacheLookup BitFasterCaching()
    {
        var result = BitFasterInstance.Set();
        CacheChecks.Written(LibraryCatalog.BitFasterCaching, result);
        return result;
    }
}


public partial class Invalidate
{
    private static readonly BitFasterOps BitFasterInstance = new();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.BitFasterCaching)]
    public bool BitFasterCaching()
    {
        var result = BitFasterInstance.Invalidate();
        CacheChecks.Removed(LibraryCatalog.BitFasterCaching, result);
        return result;
    }
}

