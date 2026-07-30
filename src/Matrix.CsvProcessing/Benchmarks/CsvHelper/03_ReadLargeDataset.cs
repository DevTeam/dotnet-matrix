using CsvHelper;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadLargeDataset
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public CsvRecord[] CsvHelper()
    {
        using var source = new StringReader(_csv);
        using var csv = new CsvReader(source, CsvHelperConfiguration.Reader);
        var result = csv.GetRecords<CsvRecord>().ToArray();
        CsvChecks.Large(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

