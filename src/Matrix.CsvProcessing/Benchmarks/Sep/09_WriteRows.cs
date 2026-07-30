using System.Globalization;
using System.Text;
using nietras.SeparatedValues;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class WriteRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sep)]
    [PayloadSize(CsvData.WriteCsv)]
    public string Sep()
    {
        var buffer = new StringBuilder(64);
        using var destination = new StringWriter(buffer, CultureInfo.InvariantCulture)
        {
            NewLine = "\n"
        };
        using (var csv = SepConfiguration.Writer.To(destination, true))
        {
            foreach (var record in _records)
            {
                using var row = csv.NewRow();
                row["Id"].Format(record.Id);
                row["Name"].Set(record.Name);
            }
        }

        var result = buffer.ToString();
        CsvChecks.WrittenCsv(LibraryCatalog.Sep, result);
        return result;
    }
}

