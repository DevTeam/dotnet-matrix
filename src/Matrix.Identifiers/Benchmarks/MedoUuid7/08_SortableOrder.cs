// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class SortableOrder
{
    private readonly global::Medo.Uuid7[] _medoValues = CreateMedoValues();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MedoUuid7)]
    public int MedoUuid7()
    {
        var ordered = 0;
        for (var index = 0; index + 1 < _medoValues.Length; index++)
        {
            if (_medoValues[index].CompareTo(_medoValues[index + 1]) < 0)
            {
                ordered++;
            }
        }

        IdentifierChecks.Ordered(LibraryCatalog.MedoUuid7, ordered, _medoValues.Length - 1);
        return ordered;
    }

    private static global::Medo.Uuid7[] CreateMedoValues()
    {
        var values = new global::Medo.Uuid7[IdentifierData.OrderingSamples];
        for (var index = 0; index < values.Length; index++)
        {
            values[index] = global::Medo.Uuid7.NewUuid7(IdentifierData.NextTimestamp());
        }

        return values;
    }
}
