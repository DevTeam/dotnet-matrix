using System.Globalization;
using FlatFiles;
using FlatFiles.TypeMapping;

namespace Matrix.CsvProcessing;

internal static class FlatFilesConfiguration
{
    public static DelimitedOptions Options { get; } = new()
    {
        Separator = ",",
        IsFirstRecordSchema = true,
        FormatProvider = CultureInfo.InvariantCulture,
        RecordSeparator = "\n"
    };

    public static IDelimitedTypeMapper<CsvRecord> RecordMapper { get; } = DefineRecordMapper();

    public static IDelimitedTypeMapper<RawCsvRow> RawRowMapper { get; } = DefineRawRowMapper();

    public static IDelimitedTypeMapper<TextCsvRow> TextRowMapper { get; } = DefineTextRowMapper();

    public static IDelimitedTypeMapper<ProductCodeHolder> ProductCodeMapper { get; } = DefineProductCodeMapper();

    public static IDelimitedTypeMapper<CsvWriteRecord> WriteMapper { get; } = DefineWriteMapper();

    private static IDelimitedTypeMapper<CsvRecord> DefineRecordMapper()
    {
        var mapper = DelimitedTypeMapper.Define<CsvRecord>();
        mapper.Property(x => x.Id).ColumnName("Id");
        mapper.Property(x => x.Name).ColumnName("Name");
        mapper.Property(x => x.Amount).ColumnName("Amount");
        mapper.Property(x => x.Active).ColumnName("Active");
        return mapper;
    }

    private static IDelimitedTypeMapper<RawCsvRow> DefineRawRowMapper()
    {
        var mapper = DelimitedTypeMapper.Define<RawCsvRow>();
        mapper.Property(x => x.Id).ColumnName("Id");
        mapper.Property(x => x.Name).ColumnName("Name");
        mapper.Property(x => x.Amount).ColumnName("Amount");
        mapper.Property(x => x.Active).ColumnName("Active");
        return mapper;
    }

    private static IDelimitedTypeMapper<TextCsvRow> DefineTextRowMapper()
    {
        var mapper = DelimitedTypeMapper.Define<TextCsvRow>();
        mapper.Property(x => x.Id).ColumnName("Id");
        mapper.Property(x => x.Text).ColumnName("Text");
        return mapper;
    }

    private static IDelimitedTypeMapper<ProductCodeHolder> DefineProductCodeMapper()
    {
        var mapper = DelimitedTypeMapper.Define<ProductCodeHolder>();
        mapper.CustomMapping(new StringColumn("Code")).WithReader(ReadProductCode);
        return mapper;
    }

    private static IDelimitedTypeMapper<CsvWriteRecord> DefineWriteMapper()
    {
        var mapper = DelimitedTypeMapper.Define(() => new CsvWriteRecord(0, string.Empty));
        mapper.Property(x => x.Id).ColumnName("Id");
        mapper.Property(x => x.Name).ColumnName("Name");
        return mapper;
    }

    private static void ReadProductCode(ProductCodeHolder entity, object? value) =>
        entity.Code = ProductCode.Parse((string)value!, CultureInfo.InvariantCulture);
}

internal sealed class ProductCodeHolder
{
    public ProductCode Code { get; set; }
}
