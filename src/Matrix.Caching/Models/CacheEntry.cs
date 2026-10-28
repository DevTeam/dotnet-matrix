namespace Matrix.Caching.Models;

/// <summary>
/// The cached value of every scenario. An immutable struct, so reading it back
/// from a cache allocates nothing and the comparison measures the cache rather
/// than the payload.
/// </summary>
public readonly record struct CacheEntry(int Id, long Ticks, int Counter);
