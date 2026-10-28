// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Nanoid)]
    public string Nanoid()
    {
        var id = NanoidDotNet.Nanoid.Generate(size: IdentifierData.RandomLength);
        IdentifierChecks.TextLength(LibraryCatalog.Nanoid, id, IdentifierData.RandomLength);
        return id;
    }
}
