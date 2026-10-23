using CsvHelper;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class StreamingRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public CsvAggregate CsvHelper()
    {
        using var csv = CsvReader.Create(new StringReader(_csv), CsvHelperConfiguration.Reader);
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        while (csv.Read())
        {
            count++;
            idSum += csv.GetInt32(0);
            amountSum += csv.GetDecimal(2);
            if (csv.GetBoolean(3))
            {
                activeCount++;
            }
        }

        var result = new CsvAggregate(count, idSum, amountSum, activeCount);
        CsvChecks.Aggregate(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

