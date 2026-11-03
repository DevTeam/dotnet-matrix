// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class AsyncRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlatFiles)]
    public async Task<CsvAggregate> FlatFiles()
    {
        var reader = FlatFilesConfiguration.RecordMapper.GetReader(
            new StringReader(_csv),
            FlatFilesConfiguration.Options);
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        while (await reader.ReadAsync())
        {
            var record = reader.Current;
            count++;
            idSum += record.Id;
            amountSum += record.Amount;
            if (record.Active)
            {
                activeCount++;
            }
        }

        var result = new CsvAggregate(count, idSum, amountSum, activeCount);
        CsvChecks.Aggregate(LibraryCatalog.FlatFiles, result);
        return result;
    }
}
