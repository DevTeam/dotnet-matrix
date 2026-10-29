// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Parse
{
    private readonly global::IdGen.IdGenerator _idGenParser = IdentifierData.CreateGenerator();

    private readonly long _idGenNumber = IdentifierData.CreateGenerator().CreateId();

    private global::IdGen.Id _idGenExpected;

    [GlobalSetup(Target = nameof(IdGen))]
    public void SetupIdGen() => _idGenExpected = _idGenParser.FromId(_idGenNumber);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IdGen)]
    public global::IdGen.Id IdGen()
    {
        var id = _idGenParser.FromId(_idGenNumber);
        IdentifierChecks.Reflects(
            LibraryCatalog.IdGen,
            id.GeneratorId == _idGenExpected.GeneratorId
            && id.SequenceNumber == _idGenExpected.SequenceNumber
            && id.DateTimeOffset == _idGenExpected.DateTimeOffset,
            "The reconstructed identifier");
        return id;
    }
}
