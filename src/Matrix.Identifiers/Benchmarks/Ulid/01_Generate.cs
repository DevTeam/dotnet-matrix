// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ulid)]
    public global::System.Ulid Ulid()
    {
        var timestamp = IdentifierData.NextTimestampMilliseconds();
        var id = global::System.Ulid.NewUlid(DateTimeOffset.FromUnixTimeMilliseconds(timestamp));
        IdentifierChecks.TextLength(LibraryCatalog.Ulid, id.ToString(), IdentifierData.UlidLength);
        IdentifierChecks.Timestamp(LibraryCatalog.Ulid, id.Time.ToUnixTimeMilliseconds(), timestamp);
        return id;
    }
}
