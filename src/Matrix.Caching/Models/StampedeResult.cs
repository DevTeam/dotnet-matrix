namespace Matrix.Caching.Models;

/// <summary>
/// Outcome of a concurrent get-or-create: how many callers asked, and how many
/// times the value factory actually ran. A cache that protects against a
/// stampede runs it exactly once.
/// </summary>
public readonly record struct StampedeResult(int Callers, int FactoryRuns, CacheEntry Entry);
