// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class StreamingRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.TinyCsvParser)]
    public CsvAggregate TinyCsvParser()
    {
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        foreach (var row in TinyCsvParserConfiguration.Records.ReadFromString(_csv))
        {
            count++;
            idSum += row.Result.Id;
            amountSum += row.Result.Amount;
            if (row.Result.Active)
            {
                activeCount++;
            }
        }

        var result = new CsvAggregate(count, idSum, amountSum, activeCount);
        CsvChecks.Aggregate(LibraryCatalog.TinyCsvParser, result);
        return result;
    }
}
