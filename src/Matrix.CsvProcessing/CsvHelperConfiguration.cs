using System.Globalization;
using CsvHelper;

namespace Matrix.CsvProcessing;

internal static class CsvHelperConfiguration
{
    public static CsvReaderOptions Reader { get; } = new()
    {
        HasHeader = true,
        FormatProvider = CultureInfo.InvariantCulture
    };

    public static CsvWriterOptions Writer { get; } = new()
    {
        HasHeader = true,
        NewLine = "\n",
        FormatProvider = CultureInfo.InvariantCulture
    };
}

