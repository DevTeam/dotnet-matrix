using nietras.SeparatedValues;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadSimpleRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sep)]
    public RawCsvRow[] Sep()
    {
        using var csv = SepConfiguration.Reader.FromText(_csv);
        var rows = new List<RawCsvRow>(3);
        foreach (var row in csv)
        {
            rows.Add(new RawCsvRow(
                row[0].ToString(),
                row[1].ToString(),
                row[2].ToString(),
                row[3].ToString()));
        }

        var result = rows.ToArray();
        CsvChecks.RawRows(LibraryCatalog.Sep, result);
        return result;
    }
}

