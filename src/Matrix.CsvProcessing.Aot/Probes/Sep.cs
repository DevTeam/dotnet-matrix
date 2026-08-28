using nietras.SeparatedValues;

namespace Matrix.CsvProcessing.Aot;

internal static class AotProbe
{
    public const string Library = "Sep";

    public const int ExpectedEvents = 1;

    private const string Csv = "Name,Value\r\nprobe,7\r\n";

    /// <summary>
    /// Reads one header and one data row from an in-memory CSV, exactly like the benchmarks'
    /// <c>ReadSimpleRows</c> scenario minus the shared fixture, and checks the parsed field values.
    /// </summary>
    public static int Run()
    {
        using var reader = Sep.New(',').Reader(options => options with { HasHeader = true }).FromText(Csv);
        foreach (var row in reader)
        {
            return row[0].ToString() == "probe" && row[1].ToString() == "7" ? 1 : 0;
        }

        return 0;
    }
}
