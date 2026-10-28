// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class GenerateBatch
{
    private readonly global::System.Ulid[] _ulidValues = new global::System.Ulid[IdentifierData.BatchSize];

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ulid)]
    public global::System.Ulid[] Ulid()
    {
        for (var index = 0; index < _ulidValues.Length; index++)
        {
            _ulidValues[index] = global::System.Ulid.NewUlid(IdentifierData.NextTimestamp());
        }

        IdentifierChecks.Batch(LibraryCatalog.Ulid, _ulidValues, IdentifierData.BatchSize);
        return _ulidValues;
    }
}
