using System.Globalization;
using TinyCsvParser;
using TinyCsvParser.Mappings;
using TinyCsvParser.Models;
using TinyCsvParser.TypeConverters;

namespace Matrix.CsvProcessing.Benchmarks;

internal static class TinyCsvParserConfiguration
{
    private static CsvOptions Options { get; } =
        new(
            Delimiter: ',',
            QuoteChar: '"',
            EscapeChar: '"',
            SkipHeader: true);

    public static readonly CsvParser<RawCsvRow> Raw =
        new(Options, new RawCsvRowMapping());

    public static readonly CsvParser<CsvRecord> Records =
        new(Options, new CsvRecordMapping());

    public static readonly CsvParser<CsvRecord> AsyncRecords =
        new(Options, new CsvRecordIndexMapping());

    public static readonly CsvParser<TextCsvRow> Text =
        new(Options, new TextCsvRowMapping());

    public static readonly CsvParser<ProductCodeRow> ProductCodes =
        new(Options, new ProductCodeRowMapping());

    private sealed class RawCsvRowMapping : CsvMapping<RawCsvRow>
    {
        public RawCsvRowMapping()
        {
            MapProperty("Id", row => row.Id);
            MapProperty("Name", row => row.Name);
            MapProperty("Amount", row => row.Amount);
            MapProperty("Active", row => row.Active);
        }
    }

    private sealed class CsvRecordMapping : CsvMapping<CsvRecord>
    {
        public CsvRecordMapping()
        {
            MapProperty("Id", row => row.Id);
            MapProperty("Name", row => row.Name);
            MapProperty("Amount", row => row.Amount);
            MapProperty("Active", row => row.Active);
        }
    }

    private sealed class CsvRecordIndexMapping : CsvMapping<CsvRecord>
    {
        public CsvRecordIndexMapping()
        {
            MapProperty(0, row => row.Id);
            MapProperty(1, row => row.Name);
            MapProperty(2, row => row.Amount);
            MapProperty(3, row => row.Active, new TrimmedBooleanConverter());
        }
    }

    private sealed class TextCsvRowMapping : CsvMapping<TextCsvRow>
    {
        public TextCsvRowMapping()
        {
            MapProperty("Id", row => row.Id);
            MapProperty(
                "Text",
                row => row.Text,
                new Rfc4180StringConverter());
        }
    }

    private sealed class ProductCodeRowMapping : CsvMapping<ProductCodeRow>
    {
        public ProductCodeRowMapping() =>
            MapProperty("Code", row => row.Code, new ProductCodeConverter());
    }

    private sealed class ProductCodeConverter : NonNullableConverter<ProductCode>
    {
        protected override bool InternalConvert(
            ReadOnlySpan<char> value,
            out ProductCode result) =>
            ProductCode.TryParse(value, CultureInfo.InvariantCulture, out result);
    }

    private sealed class Rfc4180StringConverter : NonNullableConverter<string>
    {
        protected override bool InternalConvert(
            ReadOnlySpan<char> value,
            out string result)
        {
            result = value.ToString().Replace("\"\"", "\"", StringComparison.Ordinal);
            return true;
        }
    }

    private sealed class TrimmedBooleanConverter : NonNullableConverter<bool>
    {
        protected override bool InternalConvert(
            ReadOnlySpan<char> value,
            out bool result) =>
            bool.TryParse(value.Trim(), out result);
    }
}

internal sealed class ProductCodeRow
{
    public ProductCode Code { get; set; }
}
