using System.Globalization;
using Sylvan.Data.Csv;

namespace Matrix.CsvProcessing;

internal static class SylvanConfiguration
{
    public static CsvDataReaderOptions Reader { get; } = new()
    {
        HasHeaders = true,
        Culture = CultureInfo.InvariantCulture
    };

    public static CsvDataWriterOptions Writer { get; } = new()
    {
        WriteHeaders = true,
        NewLine = "\n",
        Culture = CultureInfo.InvariantCulture
    };
}

