// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class SortableOrder
{
    private readonly global::System.Ulid[] _ulidValues = CreateUlidValues();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ulid)]
    public int Ulid()
    {
        var ordered = 0;
        for (var index = 0; index + 1 < _ulidValues.Length; index++)
        {
            if (_ulidValues[index].CompareTo(_ulidValues[index + 1]) < 0)
            {
                ordered++;
            }
        }

        IdentifierChecks.Ordered(LibraryCatalog.Ulid, ordered, _ulidValues.Length - 1);
        return ordered;
    }

    private static global::System.Ulid[] CreateUlidValues()
    {
        var values = new global::System.Ulid[IdentifierData.OrderingSamples];
        for (var index = 0; index < values.Length; index++)
        {
            values[index] = global::System.Ulid.NewUlid(IdentifierData.NextTimestamp());
        }

        return values;
    }
}
