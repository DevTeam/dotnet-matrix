namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("ReadZip64Archive", 12, "Read Zip64 Archive", "Opens and enumerates a Zip64 archive containing 65,536 entries.")]
public partial class ReadZip64Archive
{
    private readonly byte[] _archive = ZipData.Zip64Archive;
}
