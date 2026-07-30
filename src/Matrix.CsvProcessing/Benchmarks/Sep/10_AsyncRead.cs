using nietras.SeparatedValues;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class AsyncRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sep)]
    public async Task<CsvAggregate> Sep()
    {
        using var csv = await SepConfiguration.Reader.FromTextAsync(_csv);
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        await foreach (var row in csv)
        {
            count++;
            idSum += row[0].Parse<int>();
            amountSum += row[2].Parse<decimal>();
            if (row[3].Parse<bool>())
            {
                activeCount++;
            }
        }

        var result = new CsvAggregate(count, idSum, amountSum, activeCount);
        CsvChecks.Aggregate(LibraryCatalog.Sep, result);
        return result;
    }
}

