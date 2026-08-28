using StructLinq;

namespace Matrix.LinqQueries.Aot;

internal static class AotProbe
{
    public const string Library = "StructLinq";

    public const int ExpectedEvents = 1;

    private static readonly int[] Source = [1, 2, 3, 4, 5, 6];

    /// <summary>
    /// Filters and counts one small array, exactly like the benchmarks' <c>FilterCount</c>
    /// scenario minus the shared fixture, and checks the result.
    /// </summary>
    public static int Run()
    {
        var result = Source.ToStructEnumerable().Where(static n => n % 3 == 0).Count();
        return result == 2 ? 1 : 0;
    }
}
