using System.Globalization;
using NReco.Csv;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class StreamingRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NRecoCsv)]
    public CsvAggregate NRecoCsv()
    {
        var reader = new CsvReader(new StringReader(_csv), NRecoCsvConfiguration.Separator);
        reader.Read();
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        while (reader.Read())
        {
            count++;
            idSum += int.Parse(reader[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            amountSum += decimal.Parse(reader[2], NumberStyles.Number, CultureInfo.InvariantCulture);
            if (bool.Parse(reader[3]))
            {
                activeCount++;
            }
        }

        var result = new CsvAggregate(count, idSum, amountSum, activeCount);
        CsvChecks.Aggregate(LibraryCatalog.NRecoCsv, result);
        return result;
    }
}
