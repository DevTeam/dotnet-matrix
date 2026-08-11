// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FlattenSelectMany
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.HandCoded)]
    public int[] HandCoded()
    {
        var result = new int[10_000];
        var resultIndex = 0;
        for (var batchIndex = 0; batchIndex < _source.Length; batchIndex++)
        {
            var batch = _source[batchIndex];
            for (var index = 0; index < batch.Length; index++)
            {
                result[resultIndex++] = batch[index];
            }
        }

        Validate(LibraryCatalog.HandCoded, result);
        return result;
    }
}
