using System.Globalization;
using CsvHelper.Configuration;

namespace Matrix.CsvProcessing;

internal static class CsvHelperConfiguration
{
    public static CsvConfiguration Reader { get; } = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true
    };

    public static CsvConfiguration Writer { get; } = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        NewLine = "\n"
    };
}

