// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class GenerateBatch
{
    private readonly global::NUlid.Ulid[] _nulidValues = new global::NUlid.Ulid[IdentifierData.BatchSize];

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NUlid)]
    public global::NUlid.Ulid[] NUlid()
    {
        for (var index = 0; index < _nulidValues.Length; index++)
        {
            _nulidValues[index] = global::NUlid.Ulid.NewUlid(IdentifierData.NextTimestamp());
        }

        IdentifierChecks.Batch(LibraryCatalog.NUlid, _nulidValues, IdentifierData.BatchSize);
        return _nulidValues;
    }
}
