// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("DecompressEntry", 4, "Decompress Entry", "Opens and inflates one 4 MiB deflated ZIP entry.")]
public partial class DecompressEntry
{
    private readonly byte[] _archive = ZipData.LargeDeflatedArchive;
    private readonly DigestWriteStream _sink = new();
}
