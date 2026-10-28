// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class GenerateBatch
{
    private readonly global::Medo.Uuid7[] _medoValues = new global::Medo.Uuid7[IdentifierData.BatchSize];

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MedoUuid7)]
    public global::Medo.Uuid7[] MedoUuid7()
    {
        for (var index = 0; index < _medoValues.Length; index++)
        {
            _medoValues[index] = global::Medo.Uuid7.NewUuid7(IdentifierData.NextTimestamp());
        }

        IdentifierChecks.Batch(LibraryCatalog.MedoUuid7, _medoValues, IdentifierData.BatchSize);
        return _medoValues;
    }
}
