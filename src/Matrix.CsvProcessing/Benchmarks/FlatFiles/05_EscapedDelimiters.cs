// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class EscapedDelimiters
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatFiles)]
    public TextCsvRow[] FlatFiles()
    {
        var reader = FlatFilesConfiguration.TextRowMapper.GetReader(
            new StringReader(_csv),
            FlatFilesConfiguration.Options);
        var rows = new List<TextCsvRow>(2);
        while (reader.Read())
        {
            rows.Add(reader.Current);
        }

        var result = rows.ToArray();
        CsvChecks.EscapedDelimiters(LibraryCatalog.FlatFiles, result);
        return result;
    }
}
