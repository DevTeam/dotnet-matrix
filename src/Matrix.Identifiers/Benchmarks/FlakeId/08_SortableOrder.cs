// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class SortableOrder
{
    private readonly global::FlakeId.Id[] _flakeValues = CreateFlakeValues();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlakeId)]
    public int FlakeId()
    {
        var ordered = 0;
        for (var index = 0; index + 1 < _flakeValues.Length; index++)
        {
            if (_flakeValues[index].CompareTo(_flakeValues[index + 1]) < 0)
            {
                ordered++;
            }
        }

        IdentifierChecks.Ordered(LibraryCatalog.FlakeId, ordered, _flakeValues.Length - 1);
        return ordered;
    }

    private static global::FlakeId.Id[] CreateFlakeValues()
    {
        var values = new global::FlakeId.Id[IdentifierData.OrderingSamples];
        for (var index = 0; index < values.Length; index++)
        {
            values[index] = global::FlakeId.Id.Create(IdentifierData.NextTimestampMilliseconds());
        }

        return values;
    }
}
