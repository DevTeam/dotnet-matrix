// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class GenerateBatch
{
    private readonly Guid[] _uuidNextValues = new Guid[IdentifierData.BatchSize];

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.UUIDNext)]
    public Guid[] UUIDNext()
    {
        for (var index = 0; index < _uuidNextValues.Length; index++)
        {
            _uuidNextValues[index] = global::UUIDNext.Tools.UuidToolkit.CreateUuidV7FromSpecificDate(
                IdentifierData.NextTimestamp());
        }

        IdentifierChecks.Batch(LibraryCatalog.UUIDNext, _uuidNextValues, IdentifierData.BatchSize);
        return _uuidNextValues;
    }
}
