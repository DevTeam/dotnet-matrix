using nietras.SeparatedValues;
// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

public partial class StreamingRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Sep)]
    public CsvAggregate Sep()
    {
        using var csv = SepConfiguration.Reader.FromText(_csv);
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        foreach (var row in csv)
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

