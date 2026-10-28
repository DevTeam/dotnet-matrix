// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class GenerateBatch
{
    private readonly string[] _nanoidValues = new string[IdentifierData.BatchSize];

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Nanoid)]
    public string[] Nanoid()
    {
        for (var index = 0; index < _nanoidValues.Length; index++)
        {
            _nanoidValues[index] = NanoidDotNet.Nanoid.Generate(size: IdentifierData.RandomLength);
        }

        IdentifierChecks.Batch(LibraryCatalog.Nanoid, _nanoidValues, IdentifierData.BatchSize);
        return _nanoidValues;
    }
}
