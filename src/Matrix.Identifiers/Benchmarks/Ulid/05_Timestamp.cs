// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Timestamp
{
    private long _ulidTimestamp;

    private global::System.Ulid _ulidValue;

    [GlobalSetup(Target = nameof(Ulid))]
    public void SetupUlid()
    {
        _ulidTimestamp = IdentifierData.NextTimestampMilliseconds();
        _ulidValue = global::System.Ulid.NewUlid(DateTimeOffset.FromUnixTimeMilliseconds(_ulidTimestamp));
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ulid)]
    public long Ulid()
    {
        var timestamp = _ulidValue.Time.ToUnixTimeMilliseconds();
        IdentifierChecks.Timestamp(LibraryCatalog.Ulid, timestamp, _ulidTimestamp);
        return timestamp;
    }
}
