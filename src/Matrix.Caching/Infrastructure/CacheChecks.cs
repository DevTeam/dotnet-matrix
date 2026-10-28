namespace Matrix.Caching.Infrastructure;

/// <summary>
/// Validation is a promise of behaviour, not of API shape: a cache that returns a
/// wrong or stale value, or runs a value factory once per concurrent caller, fails
/// rather than scoring lower. Compiles out of benchmark builds.
/// </summary>
internal static class CacheChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void Hit(string library, CacheLookup actual) =>
        MatrixValidation.Require(library, actual.Found && actual.Entry == CacheData.Existing, Describe("the seeded entry", actual));

    [Conditional("MATRIX_VALIDATION")]
    public static void Miss(string library, CacheLookup actual) =>
        MatrixValidation.Require(library, !actual.Found, Describe("absence", actual));

    [Conditional("MATRIX_VALIDATION")]
    public static void Written(string library, CacheLookup actual) =>
        MatrixValidation.Require(library, actual.Found && actual.Entry == CacheData.Created, Describe("the entry just written", actual));

    [Conditional("MATRIX_VALIDATION")]
    public static void Removed(string library, bool gone) =>
        MatrixValidation.Require(library, gone, "The entry was still present after invalidation.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Expired(string library, CacheLookup actual) =>
        MatrixValidation.Require(library, !actual.Found, Describe("absence after the lifetime elapsed", actual));

    [Conditional("MATRIX_VALIDATION")]
    public static void Concurrent(string library, CacheLookup actual, int factoryRuns)
    {
        MatrixValidation.Require(library, actual.Found && actual.Entry == CacheData.Stampede, Describe("the stampede entry", actual));
        MatrixValidation.Require(
            library,
            factoryRuns == 1,
            "The value factory ran " + factoryRuns + " times for " + CacheData.StampedeCallers + " concurrent callers.");
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void FailSafe(string library, CacheLookup actual) =>
        MatrixValidation.Require(library, actual.Found && actual.Entry == CacheData.FailSafe, Describe("the fail-safe value", actual));

    private static string Describe(string expected, CacheLookup actual) =>
        "Expected " + expected + ", observed Found=" + actual.Found + " Entry=" + actual.Entry + ".";
}