using CsvHelper;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class WriteRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    [PayloadSize(CsvData.WriteCsv)]
    public string CsvHelper()
    {
        using var destination = new StringWriter(
            new System.Text.StringBuilder(64),
            CsvHelperConfiguration.Writer.CultureInfo);
        using (var csv = new CsvWriter(destination, CsvHelperConfiguration.Writer))
        {
            csv.WriteRecords(_records);
        }

        var result = destination.ToString();
        CsvChecks.WrittenCsv(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

