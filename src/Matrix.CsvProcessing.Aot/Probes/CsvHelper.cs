using CsvHelper;
using System.Globalization;

namespace Matrix.CsvProcessing.Aot;

internal static class AotProbe
{
    public const string Library = "CsvHelper";

    public const int ExpectedEvents = 1;

    private const string Csv = "Name,Value\r\nprobe,7\r\n";

    /// <summary>
    /// Reads one header and one data row from an in-memory CSV, exactly like the benchmarks'
    /// <c>ReadSimpleRows</c> scenario minus the shared fixture, and checks the parsed field values.
    /// </summary>
    public static int Run()
    {
        var options = new CsvReaderOptions
        {
            HasHeader = true,
            FormatProvider = CultureInfo.InvariantCulture
        };
        using var csv = CsvReader.Create(new StringReader(Csv), options);
        csv.Read();
        return csv.GetString(0) == "probe" && csv.GetString(1) == "7" ? 1 : 0;
    }
}

