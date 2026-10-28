// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ksuid)]
    public global::KSUID.Ksuid Ksuid()
    {
        var id = global::KSUID.Ksuid.Generate();
        IdentifierChecks.TextLength(LibraryCatalog.Ksuid, id.ToString(), IdentifierData.KsuidLength);
        return id;
    }
}
