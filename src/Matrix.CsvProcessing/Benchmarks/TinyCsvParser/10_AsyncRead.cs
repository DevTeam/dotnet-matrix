// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class AsyncRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.TinyCsvParser)]
    public async Task<CsvAggregate> TinyCsvParser()
    {
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        await foreach (var row in TinyCsvParserConfiguration.AsyncRecords.ReadFromStringAsync(_csv).ConfigureAwait(false))
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
