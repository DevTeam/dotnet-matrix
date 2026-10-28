// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Timestamp
{
    private long _nulidTimestamp;

    private global::NUlid.Ulid _nulidValue;

    [GlobalSetup]
    public void SetupNUlid()
    {
        _nulidTimestamp = IdentifierData.NextTimestampMilliseconds();
        _nulidValue = global::NUlid.Ulid.NewUlid(
            DateTimeOffset.FromUnixTimeMilliseconds(_nulidTimestamp));
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.NUlid)]
    public long NUlid()
    {
        var timestamp = _nulidValue.Time.ToUnixTimeMilliseconds();
        IdentifierChecks.Timestamp(LibraryCatalog.NUlid, timestamp, _nulidTimestamp);
        return timestamp;
    }
}
