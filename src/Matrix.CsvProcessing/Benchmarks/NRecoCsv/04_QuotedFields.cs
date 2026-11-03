using System.Globalization;
using NReco.Csv;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class QuotedFields
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NRecoCsv)]
    public TextCsvRow[] NRecoCsv()
    {
        var reader = new CsvReader(new StringReader(_csv), NRecoCsvConfiguration.Separator);
        reader.Read();
        var rows = new List<TextCsvRow>(2);
        while (reader.Read())
        {
            rows.Add(new TextCsvRow(int.Parse(reader[0], NumberStyles.Integer, CultureInfo.InvariantCulture), reader[1]));
        }

        var result = rows.ToArray();
        CsvChecks.Quoted(LibraryCatalog.NRecoCsv, result);
        return result;
    }
}
