// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class ReadTypedRecords
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatFiles)]
    public CsvRecord[] FlatFiles()
    {
        var reader = FlatFilesConfiguration.RecordMapper.GetReader(
            new StringReader(_csv),
            FlatFilesConfiguration.Options);
        var records = new List<CsvRecord>(3);
        while (reader.Read())
        {
            records.Add(reader.Current);
        }

        var result = records.ToArray();
        CsvChecks.Records(LibraryCatalog.FlatFiles, result);
        return result;
    }
}
