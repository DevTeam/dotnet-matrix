// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Timestamp
{
    private readonly global::IdGen.Id _idGenValue =
        IdentifierData.CreateGenerator().FromId(IdentifierData.CreateGenerator().CreateId());

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IdGen)]
    public long IdGen()
    {
        var timestamp = _idGenValue.DateTimeOffset.ToUnixTimeMilliseconds();
        IdentifierChecks.TimestampRecent(
            LibraryCatalog.IdGen,
            timestamp,
            IdentifierData.NowMilliseconds(),
            IdentifierData.ClockToleranceMilliseconds);
        return timestamp;
    }
}
