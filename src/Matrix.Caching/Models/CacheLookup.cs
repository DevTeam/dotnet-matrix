namespace Matrix.Caching.Models;

/// <summary>
/// One read result: whether the key was found and what it held. Returned as a
/// struct so a miss costs no allocation on the hot path.
/// </summary>
public readonly record struct CacheLookup(bool Found, CacheEntry Entry);
