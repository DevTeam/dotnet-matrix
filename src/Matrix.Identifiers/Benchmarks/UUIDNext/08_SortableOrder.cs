// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class SortableOrder
{
    private readonly Guid[] _uuidNextValues = CreateUuidNextValues();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.UUIDNext)]
    public int UUIDNext()
    {
        var ordered = 0;
        for (var index = 0; index + 1 < _uuidNextValues.Length; index++)
        {
            if (_uuidNextValues[index].CompareTo(_uuidNextValues[index + 1]) < 0)
            {
                ordered++;
            }
        }

        IdentifierChecks.Ordered(LibraryCatalog.UUIDNext, ordered, _uuidNextValues.Length - 1);
        return ordered;
    }

    private static Guid[] CreateUuidNextValues()
    {
        var values = new Guid[IdentifierData.OrderingSamples];
        for (var index = 0; index < values.Length; index++)
        {
            values[index] = global::UUIDNext.Tools.UuidToolkit.CreateUuidV7FromSpecificDate(
                IdentifierData.NextTimestamp());
        }

        return values;
    }
}
