// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.CuidNet)]
    public Visus.Cuid.Cuid2 CuidNet()
    {
        var id = new Visus.Cuid.Cuid2(IdentifierData.RandomLength);
        IdentifierChecks.TextLength(LibraryCatalog.CuidNet, id.ToString(), IdentifierData.RandomLength);
        return id;
    }
}
