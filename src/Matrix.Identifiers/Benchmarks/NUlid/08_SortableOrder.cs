// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class SortableOrder
{
    private readonly global::NUlid.Ulid[] _nulidValues = CreateNUlidValues();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NUlid)]
    public int NUlid()
    {
        var ordered = 0;
        for (var index = 0; index + 1 < _nulidValues.Length; index++)
        {
            if (_nulidValues[index].CompareTo(_nulidValues[index + 1]) < 0)
            {
                ordered++;
            }
        }

        IdentifierChecks.Ordered(LibraryCatalog.NUlid, ordered, _nulidValues.Length - 1);
        return ordered;
    }

    private static global::NUlid.Ulid[] CreateNUlidValues()
    {
        var values = new global::NUlid.Ulid[IdentifierData.OrderingSamples];
        for (var index = 0; index < values.Length; index++)
        {
            values[index] = global::NUlid.Ulid.NewUlid(IdentifierData.NextTimestamp());
        }

        return values;
    }
}
