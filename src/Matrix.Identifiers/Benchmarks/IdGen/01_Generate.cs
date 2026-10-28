// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    private readonly global::IdGen.IdGenerator _idGenGenerator = IdentifierData.CreateGenerator();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IdGen)]
    public long IdGen()
    {
        var before = IdentifierData.NowMilliseconds();
        var id = _idGenGenerator.CreateId();
        IdentifierChecks.TimestampAfter(
            LibraryCatalog.IdGen,
            _idGenGenerator.FromId(id).DateTimeOffset.ToUnixTimeMilliseconds(),
            before);
        return id;
    }
}
