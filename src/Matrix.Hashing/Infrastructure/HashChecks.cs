using System.Globalization;

namespace Matrix.Hashing.Infrastructure;

/// <summary>
/// A scenario fails when a library returns a different number for the same
/// algorithm. It never merely scores lower: "one algorithm per scenario" is
/// enforced here rather than trusted. See workflows/feature-contracts/hashing.md.
/// </summary>
internal static class HashChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void XxHash64(string library, ulong actual) =>
        Equal(library, actual, HashData.XxHash64Expected, "XxHash64");

    [Conditional("MATRIX_VALIDATION")]
    public static void XxHash32(string library, uint actual) =>
        Equal(library, actual, HashData.XxHash32Expected, "XxHash32");

    [Conditional("MATRIX_VALIDATION")]
    public static void XxHash3(string library, ulong actual) =>
        Equal(library, actual, HashData.XxHash3Expected, "XxHash3");

    [Conditional("MATRIX_VALIDATION")]
    public static void XxHash64Streaming(string library, ulong actual) =>
        Equal(library, actual, HashData.XxHash64Expected, "XxHash64Streaming");

    [Conditional("MATRIX_VALIDATION")]
    public static void XxHash64Large(string library, ulong actual) =>
        Equal(library, actual, HashData.XxHash64LargeExpected, "XxHash64Large");

    [Conditional("MATRIX_VALIDATION")]
    public static void Crc32(string library, uint actual) =>
        Equal(library, actual, HashData.Crc32Expected, "Crc32");

    [Conditional("MATRIX_VALIDATION")]
    public static void Blake3(string library, ulong actual) =>
        Equal(library, actual, HashData.Blake3Expected, "Blake3");

    [Conditional("MATRIX_VALIDATION")]
    public static void Blake2b(string library, ulong actual) =>
        Equal(library, actual, HashData.Blake2bExpected, "Blake2b");

    [Conditional("MATRIX_VALIDATION")]
    public static void FarmHash64(string library, ulong actual) =>
        Equal(library, actual, HashData.FarmHash64Expected, "FarmHash64");

    private static void Equal(string library, ulong actual, ulong expected, string scenario) =>
        MatrixValidation.Require(
            library,
            actual == expected,
            $"{scenario} returned {Hex(actual)}, expected {Hex(expected)}.");

    private static void Equal(string library, uint actual, uint expected, string scenario) =>
        MatrixValidation.Require(
            library,
            actual == expected,
            $"{scenario} returned {Hex(actual)}, expected {Hex(expected)}.");

    private static string Hex(ulong value) => value.ToString("X16", CultureInfo.InvariantCulture);

    private static string Hex(uint value) => value.ToString("X8", CultureInfo.InvariantCulture);
}
