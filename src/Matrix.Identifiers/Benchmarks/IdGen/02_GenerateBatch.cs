// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class GenerateBatch
{
    private readonly global::IdGen.IdGenerator _idGenGenerator = IdentifierData.CreateGenerator();

    private readonly long[] _idGenValues = new long[IdentifierData.BatchSize];

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IdGen)]
    public long[] IdGen()
    {
        for (var index = 0; index < _idGenValues.Length; index++)
        {
            _idGenValues[index] = _idGenGenerator.CreateId();
        }

        IdentifierChecks.Batch(LibraryCatalog.IdGen, _idGenValues, IdentifierData.BatchSize);
        return _idGenValues;
    }
}
