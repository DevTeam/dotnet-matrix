namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("ReadStoredEntry", 3, "Read Stored Entry", "Opens and reads one uncompressed 4 MiB ZIP entry.")]
public partial class ReadStoredEntry
{
    private readonly byte[] _archive = ZipData.LargeStoredArchive;
    private readonly DigestWriteStream _sink = new();
}
