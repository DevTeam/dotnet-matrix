using Sylvan.Data.Csv;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class AsyncRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sylvan)]
    public async Task<CsvAggregate> Sylvan()
    {
        using var source = new StringReader(_csv);
        await using var csv = await CsvDataReader.CreateAsync(source, SylvanConfiguration.Reader);
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        while (await csv.ReadAsync())
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
        CsvChecks.Aggregate(LibraryCatalog.Sylvan, result);
        return result;
    }
}

