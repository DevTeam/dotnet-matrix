// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.UUIDNext)]
    public Guid UUIDNext()
    {
        var timestamp = IdentifierData.NextTimestampMilliseconds();
        var id = global::UUIDNext.Tools.UuidToolkit.CreateUuidV7FromSpecificDate(
            DateTimeOffset.FromUnixTimeMilliseconds(timestamp));
        IdentifierChecks.TextLength(LibraryCatalog.UUIDNext, id.ToString("D"), IdentifierData.GuidLength);

        var decoded = global::UUIDNext.Tools.UuidDecoder.TryDecodeTimestamp(id, out var date);
        MatrixValidation.Require(LibraryCatalog.UUIDNext, decoded, "The timestamp could not be decoded.");
        IdentifierChecks.Timestamp(
            LibraryCatalog.UUIDNext,
            new DateTimeOffset(DateTime.SpecifyKind(date, DateTimeKind.Utc)).ToUnixTimeMilliseconds(),
            timestamp);
        return id;
    }
}
