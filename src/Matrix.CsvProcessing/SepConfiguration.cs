using System.Globalization;
using nietras.SeparatedValues;

namespace Matrix.CsvProcessing;

internal static class SepConfiguration
{
    private static readonly Sep Comma = Sep.New(',');

    public static SepReaderOptions Reader { get; } = Comma.Reader(options => options with
    {
        HasHeader = true,
        CultureInfo = CultureInfo.InvariantCulture,
        Unescape = true
    });

    public static SepWriterOptions Writer { get; } = Comma.Writer(options => options with
    {
        WriteHeader = true,
        CultureInfo = CultureInfo.InvariantCulture,
        Escape = true
    });
}

