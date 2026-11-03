using NReco.Csv;

namespace Matrix.CsvProcessing.Aot;

internal static class AotProbe
{
    public const string Library = "NReco.Csv";

    public const int ExpectedEvents = 1;

    private const string Csv = "Name,Value\nprobe,7\n";

    /// <summary>
    /// Reads one header and one data row from an in-memory CSV through the library's reader,
    /// exactly like the benchmarks' <c>ReadSimpleRows</c> scenario minus the shared fixture, and
    /// checks the parsed field values.
    /// </summary>
    public static int Run()
    {
        var reader = new CsvReader(new StringReader(Csv), ",");
        reader.Read();
        reader.Read();
        return reader[0] == "probe" && reader[1] == "7" ? 1 : 0;
    }
}
