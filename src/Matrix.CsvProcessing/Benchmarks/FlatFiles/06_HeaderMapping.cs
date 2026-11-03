using FlatFiles.TypeMapping;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class HeaderMapping
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatFiles)]
    public CsvRecord[] FlatFiles()
    {
        var reader = DelimitedTypeMapper.GetAutoMappedReader<CsvRecord>(
            new StringReader(_csv),
            FlatFilesConfiguration.Options,
            AutoMapMatcher.Default);
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
