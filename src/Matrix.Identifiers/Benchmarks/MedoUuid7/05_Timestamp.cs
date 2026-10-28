// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Timestamp
{
    private long _medoTimestamp;

    private global::Medo.Uuid7 _medoValue;

    [GlobalSetup]
    public void SetupMedoUuid7()
    {
        _medoTimestamp = IdentifierData.NextTimestampMilliseconds();
        _medoValue = global::Medo.Uuid7.NewUuid7(DateTimeOffset.FromUnixTimeMilliseconds(_medoTimestamp));
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.MedoUuid7)]
    public long MedoUuid7()
    {
        var timestamp = _medoValue.ToDateTimeOffset().ToUnixTimeMilliseconds();
        IdentifierChecks.Timestamp(LibraryCatalog.MedoUuid7, timestamp, _medoTimestamp);
        return timestamp;
    }
}
