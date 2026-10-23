using CsvHelper;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class WriteRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    [PayloadSize(CsvData.WriteCsv)]
    public string CsvHelper()
    {
        var buffer = new System.Text.StringBuilder(64);
        using var destination = new StringWriter(
            buffer,
            CsvHelperConfiguration.Writer.FormatProvider);
        using (var csv = CsvWriter.Create(destination, CsvHelperConfiguration.Writer, true))
        {
            csv.WriteHeader("Id", "Name");
            foreach (var record in _records)
            {
                csv.WriteField(record.Id);
                csv.WriteField(record.Name);
                csv.EndRecord();
            }
        }

        var result = buffer.ToString();
        CsvChecks.WrittenCsv(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

