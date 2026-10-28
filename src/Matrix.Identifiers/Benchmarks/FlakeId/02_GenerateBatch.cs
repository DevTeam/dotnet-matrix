// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class GenerateBatch
{
    private readonly global::FlakeId.Id[] _flakeValues = new global::FlakeId.Id[IdentifierData.BatchSize];

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.FlakeId)]
    public global::FlakeId.Id[] FlakeId()
    {
        for (var index = 0; index < _flakeValues.Length; index++)
        {
            _flakeValues[index] = global::FlakeId.Id.Create(IdentifierData.NextTimestampMilliseconds());
        }

        IdentifierChecks.Batch(LibraryCatalog.FlakeId, _flakeValues, IdentifierData.BatchSize);
        return _flakeValues;
    }
}
