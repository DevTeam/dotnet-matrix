using Sylvan.Data.Csv;

namespace Matrix.CsvProcessing.Aot;

internal static class AotProbe
{
    public const string Library = "Sylvan.Data.Csv";

    public const int ExpectedEvents = 1;

    private const string Csv = "Name,Value\r\nprobe,7\r\n";

    /// <summary>
    /// Reads one header and one data row from an in-memory CSV, exactly like the benchmarks'
    /// <c>ReadSimpleRows</c> scenario minus the shared fixture, and checks the parsed field values.
    /// </summary>
    public static int Run()
    {
        using var source = new StringReader(Csv);
        using var csv = CsvDataReader.Create(source);
        csv.Read();
        return csv.GetString(0) == "probe" && csv.GetString(1) == "7" ? 1 : 0;
    }
}
