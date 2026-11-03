using System.Globalization;
using System.Text;
using NReco.Csv;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class WriteRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NRecoCsv)]
    [PayloadSize(CsvData.WriteCsv)]
    public string NRecoCsv()
    {
        var buffer = new StringBuilder(64);
        using var destination = new StringWriter(buffer, CultureInfo.InvariantCulture);
        destination.NewLine = NRecoCsvConfiguration.NewLine;
        var writer = new CsvWriter(destination, NRecoCsvConfiguration.Separator);
        writer.WriteField("Id");
        writer.WriteField("Name");
        writer.NextRecord();
        foreach (var record in _records)
        {
            writer.WriteField(record.Id.ToString(CultureInfo.InvariantCulture));
            writer.WriteField(record.Name);
            writer.NextRecord();
        }

        destination.Flush();
        var result = buffer.ToString();
        CsvChecks.WrittenCsv(LibraryCatalog.NRecoCsv, result);
        return result;
    }
}
