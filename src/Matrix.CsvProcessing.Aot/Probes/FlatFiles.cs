using System.Globalization;
using FlatFiles;
using FlatFiles.TypeMapping;

namespace Matrix.CsvProcessing.Aot;

internal static class AotProbe
{
    public const string Library = "FlatFiles";

    public const int ExpectedEvents = 1;

    private const string Csv = "Name,Value\nprobe,7\n";

    /// <summary>
    /// Reads one header and one data row from an in-memory CSV through the library's typed mapper,
    /// exactly like the benchmarks' <c>ReadSimpleRows</c> scenario minus the shared fixture, and
    /// checks the parsed field values.
    /// </summary>
    public static int Run()
    {
        var options = new DelimitedOptions
        {
            Separator = ",",
            IsFirstRecordSchema = true,
            FormatProvider = CultureInfo.InvariantCulture,
            RecordSeparator = "\n"
        };
        var mapper = DelimitedTypeMapper.Define<ProbeRecord>();
        mapper.Property(x => x.Name).ColumnName("Name");
        mapper.Property(x => x.Value).ColumnName("Value");
        var reader = mapper.GetReader(new StringReader(Csv), options);
        reader.Read();
        var record = reader.Current;
        return record == new ProbeRecord("probe", 7) ? 1 : 0;
    }

    private sealed record ProbeRecord(string Name, int Value)
    {
        public ProbeRecord() : this(string.Empty, 0)
        {
        }
    }
}
