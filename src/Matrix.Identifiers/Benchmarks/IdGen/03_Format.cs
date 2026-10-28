// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Format
{
    private readonly global::IdGen.Id _idGenValue =
        IdentifierData.CreateGenerator().FromId(IdentifierData.CreateGenerator().CreateId());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IdGen)]
    public string IdGen()
    {
        var text = _idGenValue.ToString();
        IdentifierChecks.NotEmpty(LibraryCatalog.IdGen, text);
        return text;
    }
}
