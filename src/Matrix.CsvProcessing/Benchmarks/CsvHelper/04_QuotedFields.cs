using CsvHelper;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class QuotedFields
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public TextCsvRow[] CsvHelper()
    {
        using var source = new StringReader(_csv);
        using var csv = new CsvReader(source, CsvHelperConfiguration.Reader);
        var result = csv.GetRecords<TextCsvRow>().ToArray();
        CsvChecks.Quoted(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

