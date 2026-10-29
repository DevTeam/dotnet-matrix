// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Timestamp
{
    private long _uuidNextTimestamp;

    private Guid _uuidNextValue;

    [GlobalSetup(Target = nameof(UUIDNext))]
    public void SetupUUIDNext()
    {
        _uuidNextTimestamp = IdentifierData.NextTimestampMilliseconds();
        _uuidNextValue = global::UUIDNext.Tools.UuidToolkit.CreateUuidV7FromSpecificDate(
            DateTimeOffset.FromUnixTimeMilliseconds(_uuidNextTimestamp));
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.UUIDNext)]
    public long UUIDNext()
    {
        var decoded = global::UUIDNext.Tools.UuidDecoder.TryDecodeTimestamp(_uuidNextValue, out var date);
        MatrixValidation.Require(LibraryCatalog.UUIDNext, decoded, "The timestamp could not be decoded.");
        var timestamp = new DateTimeOffset(DateTime.SpecifyKind(date, DateTimeKind.Utc)).ToUnixTimeMilliseconds();
        IdentifierChecks.Timestamp(LibraryCatalog.UUIDNext, timestamp, _uuidNextTimestamp);
        return timestamp;
    }
}
