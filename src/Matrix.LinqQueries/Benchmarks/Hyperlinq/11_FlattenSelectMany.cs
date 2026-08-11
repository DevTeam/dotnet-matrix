using NetFabric.Hyperlinq;
// ReSharper disable CheckNamespace
namespace Matrix.LinqQueries.Benchmarks;

public partial class FlattenSelectMany
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Hyperlinq)]
    public int[] Hyperlinq()
    {
        var result = _source.SelectMany<
                int[],
                NetFabric.Hyperlinq.ReadOnlyList.ValueEnumerableWrapper<int>,
                NetFabric.Hyperlinq.ReadOnlyList.ValueEnumerableWrapper<int>.Enumerator,
                int>(static batch => batch.AsValueEnumerable())
            .ToArray();
        Validate(LibraryCatalog.Hyperlinq, result);
        return result;
    }
}
