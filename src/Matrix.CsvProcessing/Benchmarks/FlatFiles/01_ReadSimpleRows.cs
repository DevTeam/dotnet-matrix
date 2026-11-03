using FlatFiles;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadSimpleRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatFiles)]
    public RawCsvRow[] FlatFiles()
    {
        var reader = FlatFilesConfiguration.RawRowMapper.GetReader(
            new StringReader(_csv),
            FlatFilesConfiguration.Options);
        var rows = new List<RawCsvRow>(3);
        while (reader.Read())
        {
            rows.Add(reader.Current);
        }

        var result = rows.ToArray();
        CsvChecks.RawRows(LibraryCatalog.FlatFiles, result);
        return result;
    }
}
