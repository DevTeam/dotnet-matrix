using System.Globalization;
using System.Text;
using FlatFiles.TypeMapping;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class WriteRows
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatFiles)]
    [PayloadSize(CsvData.WriteCsv)]
    public string FlatFiles()
    {
        var buffer = new StringBuilder(64);
        using var destination = new StringWriter(buffer, CultureInfo.InvariantCulture);
        var writer = FlatFilesConfiguration.WriteMapper.GetWriter(
            destination,
            FlatFilesConfiguration.Options);
        writer.WriteSchema();
        writer.WriteAll(_records);

        var result = buffer.ToString();
        CsvChecks.WrittenCsv(LibraryCatalog.FlatFiles, result);
        return result;
    }
}
