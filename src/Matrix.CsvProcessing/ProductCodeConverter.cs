using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Matrix.CsvProcessing;

internal sealed class ProductCodeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(
        string? text,
        IReaderRow row,
        MemberMapData memberMapData) =>
        ProductCode.Parse(text ?? string.Empty, row.Configuration.CultureInfo);
}

