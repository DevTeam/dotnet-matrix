// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NUlid)]
    public global::NUlid.Ulid NUlid()
    {
        var timestamp = IdentifierData.NextTimestampMilliseconds();
        var id = global::NUlid.Ulid.NewUlid(DateTimeOffset.FromUnixTimeMilliseconds(timestamp));
        IdentifierChecks.TextLength(LibraryCatalog.NUlid, id.ToString(), IdentifierData.UlidLength);
        IdentifierChecks.Timestamp(LibraryCatalog.NUlid, id.Time.ToUnixTimeMilliseconds(), timestamp);
        return id;
    }
}
