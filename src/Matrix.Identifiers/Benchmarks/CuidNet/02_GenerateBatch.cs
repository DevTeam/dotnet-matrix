// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class GenerateBatch
{
    private readonly Visus.Cuid.Cuid2[] _cuidNetValues = new Visus.Cuid.Cuid2[IdentifierData.BatchSize];

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CuidNet)]
    public Visus.Cuid.Cuid2[] CuidNet()
    {
        for (var index = 0; index < _cuidNetValues.Length; index++)
        {
            _cuidNetValues[index] = new Visus.Cuid.Cuid2(IdentifierData.RandomLength);
        }

        IdentifierChecks.Batch(LibraryCatalog.CuidNet, _cuidNetValues, IdentifierData.BatchSize);
        return _cuidNetValues;
    }
}
