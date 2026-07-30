using Sylvan.Data;
using Sylvan.Data.Csv;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class WriteRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sylvan)]
    [PayloadSize(CsvData.WriteCsv)]
    public string Sylvan()
    {
        using var destination = new StringWriter(
            new System.Text.StringBuilder(64),
            SylvanConfiguration.Writer.Culture);
        using (var source = _records.AsDataReader())
        using (var csv = CsvDataWriter.Create(destination, SylvanConfiguration.Writer))
        {
            csv.Write(source);
        }

        var result = destination.ToString();
        CsvChecks.WrittenCsv(LibraryCatalog.Sylvan, result);
        return result;
    }
}

