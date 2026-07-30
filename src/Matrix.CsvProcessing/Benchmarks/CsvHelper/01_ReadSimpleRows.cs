using CsvHelper;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadSimpleRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public RawCsvRow[] CsvHelper()
    {
        using var source = new StringReader(_csv);
        using var csv = new CsvReader(source, CsvHelperConfiguration.Reader);
        csv.Read();
        csv.ReadHeader();
        var rows = new List<RawCsvRow>(3);
        while (csv.Read())
        {
            rows.Add(new RawCsvRow(
                csv.GetField(0)!,
                csv.GetField(1)!,
                csv.GetField(2)!,
                csv.GetField(3)!));
        }

        var result = rows.ToArray();
        CsvChecks.RawRows(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

