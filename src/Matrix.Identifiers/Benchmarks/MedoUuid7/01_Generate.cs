// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MedoUuid7)]
    public global::Medo.Uuid7 MedoUuid7()
    {
        var timestamp = IdentifierData.NextTimestampMilliseconds();
        var id = global::Medo.Uuid7.NewUuid7(DateTimeOffset.FromUnixTimeMilliseconds(timestamp));
        IdentifierChecks.TextLength(LibraryCatalog.MedoUuid7, id.ToString(), IdentifierData.GuidLength);
        IdentifierChecks.Timestamp(LibraryCatalog.MedoUuid7, id.ToDateTimeOffset().ToUnixTimeMilliseconds(), timestamp);
        return id;
    }
}
