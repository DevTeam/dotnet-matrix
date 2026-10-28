// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Timestamp
{
    private readonly global::KSUID.Ksuid _ksuidValue =
        new(new byte[16], IdentifierData.KsuidTimestamp);

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Ksuid)]
    public long Ksuid()
    {
        var timestamp = (long)_ksuidValue.GetUnixTimestamp();
        IdentifierChecks.Timestamp(
            LibraryCatalog.Ksuid,
            timestamp,
            IdentifierData.KsuidEpochSeconds + IdentifierData.KsuidTimestamp);
        return timestamp;
    }
}
