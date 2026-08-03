namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("ExtractManySmallEntries", 5, "Extract Many Small Entries", "Opens and reads all 1,000 small entries from a ZIP archive.")]
public partial class ExtractManySmallEntries
{
    private readonly byte[] _archive = ZipData.ManySmallArchive;
    private readonly DigestWriteStream _sink = new();
}
