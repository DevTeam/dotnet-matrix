using Sylvan.Data.Csv;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadSimpleRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sylvan)]
    public RawCsvRow[] Sylvan()
    {
        using var source = new StringReader(_csv);
        using var csv = CsvDataReader.Create(source, SylvanConfiguration.Reader);
        var rows = new List<RawCsvRow>(3);
        while (csv.Read())
        {
            rows.Add(new RawCsvRow(
                csv.GetString(0),
                csv.GetString(1),
                csv.GetString(2),
                csv.GetString(3)));
        }

        var result = rows.ToArray();
        CsvChecks.RawRows(LibraryCatalog.Sylvan, result);
        return result;
    }
}

