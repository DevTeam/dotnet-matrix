// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class GenerateBatch
{
    private readonly global::KSUID.Ksuid[] _ksuidValues = new global::KSUID.Ksuid[IdentifierData.BatchSize];

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ksuid)]
    public global::KSUID.Ksuid[] Ksuid()
    {
        for (var index = 0; index < _ksuidValues.Length; index++)
        {
            _ksuidValues[index] = global::KSUID.Ksuid.Generate();
        }

        IdentifierChecks.Batch(LibraryCatalog.Ksuid, _ksuidValues, IdentifierData.BatchSize);
        return _ksuidValues;
    }
}
