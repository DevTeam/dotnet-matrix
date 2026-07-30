using CsvHelper;

namespace Matrix.CsvProcessing.Benchmarks;

public partial class StreamingRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CsvHelper)]
    public CsvAggregate CsvHelper()
    {
        using var source = new StringReader(_csv);
        using var csv = new CsvReader(source, CsvHelperConfiguration.Reader);
        csv.Read();
        csv.ReadHeader();
        var count = 0;
        long idSum = 0;
        decimal amountSum = 0;
        var activeCount = 0;
        while (csv.Read())
        {
            count++;
            idSum += csv.GetField<int>(0);
            amountSum += csv.GetField<decimal>(2);
            if (csv.GetField<bool>(3))
            {
                activeCount++;
            }
        }

        var result = new CsvAggregate(count, idSum, amountSum, activeCount);
        CsvChecks.Aggregate(LibraryCatalog.CsvHelper, result);
        return result;
    }
}

